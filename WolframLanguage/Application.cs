using System;
using System.Threading.Tasks;
using System.IO;
using Wolfram.NETLink;
using WolframLanguage.Properties;
using System.Collections.Generic;

namespace WolframLanguage
{
    /// <summary>
    /// The Application class holds a Wolfram System Kernel and API calls shared amongst the scope and child activities.
    /// </summary>
    public class Application : IAsyncInitialization, IDisposable
    {
        #region Properties

        private IKernelLink Kernel { get; set; }
        private bool DoneInitializing { get; set; } = false;
        private string KernelPath { get; set; }

        private string[] KernelArgs { get; set; }

        private Dictionary<Type, Func<dynamic>> @switch;
        #endregion


        #region Constructors

        // Creates a new Application using the provided credentials
        public Application(string kernelPath, string[] kernelArgs)
        {
            if (string.IsNullOrEmpty(kernelPath)) throw new ArgumentNullException("kernelPath", "A path to the MathKernel.exe is required");
            if (!File.Exists(kernelPath)) throw new ArgumentOutOfRangeException("kernelPath", "The MathKernel.exe path you specified does not exist.");
            KernelPath = kernelPath;
            KernelArgs = kernelArgs;
        }

        // Allows Initialization (the step right after constructor runs) to be asynchronous
        public Task Initialization => InitializeAsync();

        // Asynchronously creates an authenticated client to make all API calls
        private async Task InitializeAsync()
        {
            await Task.Run(() => CreateKernel());
        }

        // Once authentication is complete, creates a reusable HTTP Client
        private void CreateKernel()
        {
            Console.WriteLine("Creating Kernel");

            try
            {
                if (KernelArgs.Length == 0) //Optional param KernelArgs default launch settings
                {
                    KernelArgs = new[] { "-linkmode", "launch", "-linkname", KernelPath };
                }
                Kernel = MathLinkFactory.CreateKernelLink(KernelArgs);
                Kernel.WaitAndDiscardAnswer();
                SetupTypeDict();
                DoneInitializing = true;
                Console.WriteLine("Kernel created");

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
            @switch = new Dictionary<Type, Func<dynamic>>
                {
                    { typeof(int), () => Kernel.GetInteger() },
                    { typeof(int[]), () => Kernel.GetInt32Array() },
                    { typeof(double), () => Kernel.GetDouble() },
                    { typeof(double[]), () => Kernel.GetDoubleArray() },
                    { typeof(string), () => Kernel.GetString() },
                    { typeof(string[]), () => Kernel.GetStringArray() },
                    { typeof(bool), () => Kernel.GetBoolean() },
                    { typeof(bool[]), () => Kernel.GetBooleanArray() },
                    { typeof(byte[]), () => Kernel.GetByteArray() },
                    { typeof(object[]), () => Kernel.GetComplexArray() },
                    { typeof(decimal), () => Kernel.GetDecimal() },
                    { typeof(decimal[]), () => Kernel.GetDecimalArray() },
                    { typeof(Expr), () => Kernel.GetExpr() },
                    { typeof(object), () => Kernel.GetObject() },
                    { typeof(float[]), () => Kernel.GetSingleArray() }
                };
        }
        #endregion

        public bool Ready => Kernel != null && DoneInitializing;

        #region Action Calls

        public string EvaluateToOutputForm(string expr) => Kernel.EvaluateToOutputForm(expr, 0);

        public string EvaluateToInputForm(string expr) => Kernel.EvaluateToInputForm(expr, 0);
        public System.Drawing.Image EvaluateToImage(string expr, int width, int height) => Kernel.EvaluateToImage(expr, width, height);

        public Expr PeekExpr() => Kernel.PeekExpr();
        
        public T Evaluate<T>(string expr, Func<T> callback)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            return callback();
        }

        private T Evaluate<T>(string expr, Func<dynamic> callback)
        {
            Kernel.Evaluate(expr);
            Kernel.WaitForAnswer();
            return callback();
        }

        public T Evaluate<T>(string expr)
        {
            Console.WriteLine($"Evaluating: {expr}");
            if (@switch.ContainsKey(typeof(T)))
            {
                @switch.TryGetValue(typeof(T), out Func<dynamic> callback);
                return Evaluate<T>(expr, callback);
            }
            throw new InvalidOperationException("Tried to evaluate a type which does not have a bridge method.");
        }

        #endregion

        #region Helpers

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Kernel.Close();
                }

                KernelPath = null;

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
