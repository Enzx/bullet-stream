namespace BulletSteam.GameFramework.Logging
{
    public static class Log
    {
        public static void Info(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        
        public static void Warning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        
        public static void Error(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
        
        public static void Exception(System.Exception exception)
        {
            UnityEngine.Debug.LogException(exception);
        }
        
        public static void Info(string format, params object[] args)
        {
            UnityEngine.Debug.LogFormat(format, args);
        }
         
        public static void Warning(string format, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(format, args);
        }
        
        public static void Error(string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
        }
        
        public static void Exception(System.Exception exception, string format, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(format, args);
            UnityEngine.Debug.LogException(exception);
        }
        
        public static void Assert(bool condition, object message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }
        
        public static void Assert(bool condition, string format, params object[] args)
        {
            UnityEngine.Debug.AssertFormat(condition, format, args);
        }
        
        
         
    }
}