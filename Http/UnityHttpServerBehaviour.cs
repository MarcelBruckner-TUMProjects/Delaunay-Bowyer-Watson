using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Linq;
using System.IO;
using System.Reflection;
using StreamSocketHttpServer;
#if UNITY_WSA && !UNITY_EDITOR
using System.Threading.Tasks;
#endif

public class UnityHttpServerBehaviour : MonoBehaviour {

    class UnityHttpServer : HttpServer {

        public string DocumentRoot;
        public Dictionary<string, string> mimeMmappings;

          public UnityHttpServer(int port, string path = "www") : base(port) {
            DocumentRoot = Application.streamingAssetsPath + "/" + path;
            mimeMmappings = LoadMimeMappings();
          }

        Dictionary<string, string> LoadMimeMappings() {
            Dictionary<string, string> mappings = new Dictionary<string, string>();
            mappings.Add(".js", "application/x-javascript");
            mappings.Add(".css", "text/css");
            mappings.Add(".png", "image/png");
            mappings.Add(".gif", "image/gif");
            mappings.Add(".jpeg", "image/jpeg");
            mappings.Add(".jpg", "image/jpeg");
            mappings.Add(".tif", "image/tiff");
            mappings.Add(".tiff", "image/tiff");
            return mappings;
        }

#if UNITY_WSA && !UNITY_EDITOR
        public override async Task HandleRequest(HttpRequest request, HttpResponse response) {
            // Call back onto the Unity main thread
            UnityEngine.WSA.Application.InvokeOnAppThread(() => {
                HandleHttpRequest(request, response);
            }, true);
        }
#endif

        public void HandleHttpRequest(HttpRequest request, HttpResponse response) {
            Debug.LogFormat("HTTP {0} {1}", request.Method, request.Url);

            bool handled = DispatchRequestHandlerForRoute(request.Url, request.Method, request, response);

            // if not handled, we'll try and load it from streaming assets
            if (!handled) {
                string requestPath = request.Url;
                string path = Application.streamingAssetsPath + "/www" + requestPath;
                if (File.Exists(path)) {

                    // load content 
                    byte[] bytes = File.ReadAllBytes(path);
                    response.BodyData = bytes;
                    response.ContentLength = bytes.Length;
                    response.StatusCode = HttpStatusCode.Ok;

                    // set content type
                    string extension = Path.GetExtension(path).ToString().ToLower();
                    if (mimeMmappings.ContainsKey(extension)) {
                        response.ContentType = mimeMmappings[extension];
                    } else {
                        response.ContentType = "application/octet-stream";
                    }
                }
            }
        }
    

        bool DispatchRequestHandlerForRoute(string route, string verb, HttpRequest request, HttpResponse response) { 

            // search for a handler on all game objects in the scene
            MonoBehaviour[] sceneActive = FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour mono in sceneActive) {
                Type type = mono.GetType();
#if UNITY_WSA && !UNITY_EDITOR
                if (type.GetTypeInfo().GetCustomAttributes(typeof(UnityHttpServerAttribute), true).ToArray().Length > 0) {
#else
                if (type.GetCustomAttributes(typeof(UnityHttpServerAttribute), true).Length > 0) {
#endif
                    foreach (MethodInfo method in type.GetMethods()) {
                        object[] custom_attributes = method.GetCustomAttributes(typeof(UnityHttpRouteAttribute), false).ToArray();
                        if (custom_attributes.Length > 0) {
                            UnityHttpRouteAttribute attribute = custom_attributes[0] as UnityHttpRouteAttribute;
                            if (attribute.Route == route.ToUpper() && attribute.Verb == verb) {
                                method.Invoke(mono, new object[] { request, response });
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }


    public int Port = 8000;
    UnityHttpServer httpServer;

    void Start() {

        httpServer = new UnityHttpServer(Port);
        httpServer.Start();
    }
}

