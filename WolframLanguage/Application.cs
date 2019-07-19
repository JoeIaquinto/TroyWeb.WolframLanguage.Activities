using System;
using System.Threading.Tasks;
using System.IO;
using Wolfram.NETLink;
using WolframLanguage.Properties;
using System.Collections.Generic;
using Microsoft.Win32;

namespace WolframLanguage
{
    /// <summary>
    /// The Application class holds a Wolfram System Kernel and API calls shared amongst the scope and child activities.
    /// </summary>
    public class Application : IAsyncInitialization, IDisposable
    {
        #region Properties

        private IKernelLink Kernel { get; set; }
        private bool DoneInitializing { get; set; }
        private string KernelPath { get; set; }

        private string[] KernelArgs { get; set; }

        private bool EnableObjectReferences { get; set; }

        private static Dictionary<Type, Func<dynamic>> _switch;

        public IKernelLink KernelLink => Kernel;

        #endregion


        #region Constructors

        // Creates a new Application using the provided credentials
        public Application(string kernelPath, string[] kernelArgs, bool enableObjectReferences)
        {
            if (string.IsNullOrWhiteSpace(kernelPath))
            {
                kernelPath = GetApplicationPath(@"MathKernel.exe");
                Console.WriteLine(@"Kernel Path found to be: " + kernelPath);
                if (string.IsNullOrWhiteSpace(kernelPath)) throw new ArgumentOutOfRangeException(nameof(kernelPath), Resources.Application_Application_The_MathKernel_exe_path_you_specified_does_not_exist_);
            }

            if (!File.Exists(kernelPath)) throw new ArgumentOutOfRangeException(nameof(kernelPath), Resources.Application_Application_The_MathKernel_exe_path_you_specified_does_not_exist_);
            KernelPath = kernelPath;
            KernelArgs = kernelArgs;
            EnableObjectReferences = enableObjectReferences;
        }

        public Application(IKernelLink kernel)
        {
            Kernel = kernel;
            DoneInitializing = true;
        }

        // Allows Initialization (the step right after constructor runs) to be asynchronous
        public Task Initialization => InitializeAsync();

        private async Task InitializeAsync()
        {
            await Task.Run(CreateKernel);
        }

        private void CreateKernel()
        {
            try
            {
                if (KernelArgs is null || KernelArgs.Length == 0) //Optional param KernelArgs default launch settings
                {
                    KernelArgs = new[] {@"-linkmode", @"launch", @"-linkname", KernelPath};
                }
                
                Console.WriteLine(Resources.Application_CreateKernel_Creating_Kernel);
                Kernel = MathLinkFactory.CreateKernelLink(KernelArgs);
                Kernel.WaitAndDiscardAnswer();

                SetupTypeDict();

                if (EnableObjectReferences)
                {
                    Console.WriteLine(Resources.Application_CreateKernel_Enabling_Object_References_);
                    var needNetLinkExpr = new Expr(ExpressionType.Function, @"Needs");
                    needNetLinkExpr = new Expr(needNetLinkExpr, @"NETLink`");
                    Kernel.Evaluate(needNetLinkExpr);
                    Kernel.WaitAndDiscardAnswer();
                    Kernel.EnableObjectReferences();
                    Kernel.WaitAndDiscardAnswer();
                    Kernel.Evaluate(@"InstallNET[];");
                    Kernel.WaitAndDiscardAnswer();
                    if (Kernel.LastError != null)
                    {
                        throw Kernel.LastError;
                    }
                }

                DoneInitializing = true;
                Console.WriteLine(Resources.Application_CreateKernel_Kernel_created);
            }
            catch (Exception eq)
            {
                throw new CustomException(Resources.Application_CreateKernel_FailException, eq);
            }
        }

        #endregion

        #region Info Calls

        private void SetupTypeDict()
        {
            _switch = new Dictionary<Type, Func<dynamic>>
            {
                {typeof(int), () => Kernel.GetInteger()},
                {typeof(int[]), () => Kernel.GetInt32Array()},
                {typeof(double), () => Kernel.GetDouble()},
                {typeof(double[]), () => Kernel.GetDoubleArray()},
                {typeof(string), () => Kernel.GetString()},
                {typeof(string[]), () => Kernel.GetStringArray()},
                {typeof(bool), () => Kernel.GetBoolean()},
                {typeof(bool[]), () => Kernel.GetBooleanArray()},
                {typeof(byte[]), () => Kernel.GetByteArray()},
                {typeof(object[]), () => Kernel.GetComplexArray()},
                {typeof(decimal), () => Kernel.GetDecimal()},
                {typeof(decimal[]), () => Kernel.GetDecimalArray()},
                {typeof(Expr), () => Kernel.GetExpr()},
                {typeof(object), () => Kernel.GetObject()},
                {typeof(float[]), () => Kernel.GetSingleArray()}
            };
        }

        #endregion

        public bool Ready => Kernel != null && DoneInitializing;

        public Exception CheckError => Kernel.Error != 0 ? new ApplicationException(Kernel.ErrorMessage) : null;

        public Exception LastError => Kernel.LastError;

        #region Action Calls

        public string EvaluateToOutputForm(dynamic expr)
        {
            var result = Kernel.EvaluateToOutputForm(expr, 0);
            if (result is null && Kernel.LastError != null)
            {
                throw Kernel.LastError;
            }

            return result;
        }

        public string EvaluateToInputForm(dynamic expr)
        {
            var result = Kernel.EvaluateToInputForm(expr, 0);
            if (result is null && Kernel.LastError != null)
            {
                throw Kernel.LastError;
            }

            return result;
        }

        public System.Drawing.Image EvaluateToImage(dynamic expr, int width, int height)
        {
            var result = Kernel.EvaluateToImage(expr, width, height);
            if (result is null && Kernel.LastError != null)
            {
                throw Kernel.LastError;
            }

            return result;
        }

        public Expr PeekExpr() => Kernel.PeekExpr();

        public T Evaluate<T>(string expr, Func<T> callback)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            IfCheckErrorThrow();
            return callback();
        }

        private void IfCheckErrorThrow()
        {
            if (CheckError != null)
            {
                throw CheckError;
            }
        }

        public Expr Evaluate(string expr)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            IfCheckErrorThrow();
            return Kernel.GetExpr();
        }

        public Expr Evaluate(Expr expr)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            IfCheckErrorThrow();
            return Kernel.GetExpr();
        }

        private T Evaluate<T>(string expr, Func<dynamic> callback)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            IfCheckErrorThrow();
            return callback();
        }

        public T Evaluate<T>(string expr)
        {
            Console.WriteLine(Resources.Application_Evaluate_Evaluating___0_, expr);
            if (!_switch.ContainsKey(typeof(T)))
                throw new InvalidOperationException(Resources.Application_Evaluate_No_Type);
            _switch.TryGetValue(typeof(T), out var callback);
            return Evaluate<T>(expr, callback);
        }

        public Expr GetExpr()
        {
            IfCheckErrorThrow();
            return Kernel.GetExpr();
        }

        public void Put(dynamic e)
        {
            if (!EnableObjectReferences && e.GetType() == typeof(object)) throw new Exception(Resources.Application_PutReference_Must_Enable_Object_References);
            Kernel.Put(e);
        }

        public void PutReference(object o, Type t = null)
        {
            if (!EnableObjectReferences) throw new Exception(Resources.Application_PutReference_Must_Enable_Object_References);
            if (t is null)
            {
                Kernel.PutReference(o);
            }
            else
            {
                Kernel.PutReference(o, t);
            }
        }

        public void PutFunction(string f, int numArgs)
        {
            Kernel.PutFunction(f, numArgs);
        }

        public void PutFunction(string f, object[] args)
        {
            Kernel.PutFunctionAndArgs(f, args);
        }

        public void EndPacket() => Kernel.EndPacket();

        public T GetValue<T>()
        {
            if (!_switch.ContainsKey(typeof(T)))
                throw new InvalidOperationException(Resources.Application_Evaluate_No_Type);
            _switch.TryGetValue(typeof(T), out var callback);
            if (callback is null) throw new InvalidOperationException(Resources.Application_Evaluate_No_Type);
            return callback();
        }

        public PacketType WaitForAnswer() => Kernel.WaitForAnswer();

        public void PutNext(ExpressionType t) => Kernel.PutNext(t);
        public void PutArgCount(int i) => Kernel.PutArgCount(i);

        public void PutSymbol(string s) => Kernel.PutSymbol(s);

        public void PutSize(int i) => Kernel.PutSize(i);

        public void PutData(byte[] b) => Kernel.PutData(b);

        public void Flush() => Kernel.Flush();

        public void NewPacket() => Kernel.NewPacket();

        #endregion

        #region Helpers

        public static string ApplyTimeConstraint(string expr, int timeout)
        {
            if (timeout <= 0) return expr;
            return @"TimeConstrained[" + expr + @", " + timeout + @"]";
        }

        public static Expr ApplyTimeConstraint(Expr expr, int timeout)
        {
            if (timeout <= 0) return expr;
            var tExpr = new Expr(ExpressionType.Function, @"TimeConstrained");
            return new Expr(tExpr, expr, timeout);
        }

        private static string GetApplicationPath(string exeName)
        {
            try
            {
                var ourKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                ourKey = ourKey.OpenSubKey(@"Software\Wolfram Research\Installations", false);
                if (ourKey == null) return string.Empty;
                foreach (var subKeyName in ourKey.GetSubKeyNames())
                {
                    var key = ourKey.OpenSubKey(subKeyName);
                    var path = key?.GetValue("ExecutablePath").ToString();
                    if (key != null && (path.ToLower().Contains(@"kernel.exe") || path.ToLower().Contains(@"script.exe")))
                    {
                        return path.Substring(0, path.LastIndexOf('\\')) + @"\\" + exeName;
                    }
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
                Console.WriteLine(Resources.Application_Dispose_Closing_Kernel_now_);
                Kernel.Close();
            }

            KernelPath = null;

            _disposedValue = true;
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
