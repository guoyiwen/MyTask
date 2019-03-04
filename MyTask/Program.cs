using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTask
{
    class Program
    {
        public static AutoResetEvent AREstop1 = new AutoResetEvent(false);
        static void Main(string[] args)
        {
            //Task实例化的都是后台线程，如果要更改为前台线程，需要再方法里面修改


            #region Task任务 使用线程池
            //{
            //    //    //实例化任务,必须手动启动,注意，方法是不能带参数的
            //Task TaskFirst = new Task(Method1);

            //    //Status可以标识当前任务的状态
            //    //Created：表示默认初始化任务，但是“工厂创建的”实例直接跳过。
            //    //WaitingToRun: 这种状态表示等待任务调度器分配线程给任务执行。
            //    //RanToCompletion：任务执行完毕。
            //    Console.WriteLine("TaskFirst的状态:{0}", TaskFirst.Status);

            //TaskFirst.Start();

            //    Console.WriteLine("TaskFirst的状态:{0}", TaskFirst.Status);
            //}

            //    //工厂创建的直接执行
            //Task TaskSecond = Task.Factory.StartNew(() =>
            //{

            //    Console.WriteLine("----这是不带参数方法2----");
            //    Console.WriteLine(DateTime.Now);
            //    Console.WriteLine("----方法2结束----");
            //});

            //使用这种方法删除任务
            //CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
            //Task.Factory.StartNew(() =>
            //{

            //    Console.WriteLine("----这是要删除方法4----");
            //    Console.WriteLine(DateTime.Now);
            //    Console.WriteLine("----要删除方法结束----");
            //}, cancelTokenSource.Token);

            //cancelTokenSource.Cancel();



            //    //流程控制
            //{
            //    //没有加标识的默认使用线程池创建，若主线程结束自动结束，所以需要先堵塞主线程
            //    //AREstop1.WaitOne();

            //    //或者使用阻塞
            //    //Task.WaitAll(TaskFirst, TaskSecond);

            //    //也可以使用Wait()等待单个线程，你会发现下面TaskFirst的状态的状态为Running,因为主线程开始运行了，而线程TaskFirst还在运行中
            //    //TaskSecond.Wait();

            //    //Task.WaitAny 只要数组中有一个执行完毕，就继续执行主线程
            //    Task.WaitAny(TaskFirst, TaskSecond);

            //    //继续执行，在TaskFirst任务结束后继续执行，此时TaskFirst已经结束。记得加Wait()，否则主线程结束就直接结束了。
            //    TaskFirst.ContinueWith(NewTask =>
            //    {
            //        Console.WriteLine("----这是不带参数方法3----");
            //        Console.WriteLine(DateTime.Now);
            //        Console.WriteLine("TaskFirst的状态:{0}", TaskFirst.Status);
            //        Console.WriteLine("----方法3结束----");
            //    }).Wait();

            //}

            //Console.WriteLine("TaskFirst的状态:{0}", TaskFirst.Status);
            #endregion


            #region Task任务 使用线程
            {
                ////实例化任务,必须手动启动,注意，方法是不能带参数的
                //Task TaskFirst = new Task(Method1, TaskCreationOptions.LongRunning);
                //TaskFirst.Start();
            }
            #endregion
            
             
            
            #region Task任务 带参数
            {
                //Task<int> TaskFirst = new Task<int>(((x) => { return (int)(x); }), 10);
                //TaskFirst.Start();
                //Console.WriteLine(" result ={0}", TaskFirst.Result);

                //Task<string> TaskSecond = Task<string>.Factory.StartNew(new Func<object, string>(x => { return $"This is {x}"; }), 10);
                //Console.WriteLine(" result ={0}", TaskSecond.Result);
            }
            #endregion

            //Console.WriteLine("----这是主线程----");
            //Console.WriteLine(DateTime.Now);
            //Console.WriteLine("----主线程结束----");


            List<ModelDemo> list = new List<ModelDemo>();
            for (int i = 0; i < 1000; i++)
            {
                var demo=new ModelDemo{ Id = i };
                list.Add(demo); 
            }


            string message = string.Empty;
            var listGroup =  Test<ModelDemo>.AverageAssign(list,3);
            //信号量
            var tokenSource = new CancellationTokenSource();
            //先添加进Task数组,然后一起启动异步
            //TaskFactory taskFactory = new TaskFactory();
            //List<Task> taskList = new List<Task>();
            //taskList.Add(taskFactory.StartNew(() =>
            //{}
            // Task.WaitAll(taskList.ToArray());
            try
            {

                foreach (var group in listGroup)
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (tokenSource.Token.IsCancellationRequested)
                        {
                            Console.WriteLine("日志");
                            return;
                        }

                        //要执行的方法
                        Print(group);

                    }, tokenSource.Token);
                     
                }
                 
            }
            catch (AggregateException aex)
            {
                tokenSource.Cancel();
                foreach (var item in aex.InnerExceptions)
                {
                    message += item.Message;
                }
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }





            Console.ReadLine();

             

        }

        
        static  void Print(List<ModelDemo> model)
        {
            foreach (var item in model)
            {
                Console.WriteLine(item.Id);
            }
        }

        public class ModelDemo
        {
            public int Id { get; set; }
            public DateTime CreateTime { get; set; }
        }

   
        //C# 6.0只读赋值
        static object Locker { get; } = new object();
        static void Method1()
        {
            lock (Locker)
            {
                Thread.CurrentThread.IsBackground = false;
                Thread.Sleep(1000);
                Console.WriteLine("----这是带参数方法1----");
                Console.WriteLine(DateTime.Now);
                Console.WriteLine("----方法1结束----");
                //AREstop1.Set();
            }
        }


    }
}
