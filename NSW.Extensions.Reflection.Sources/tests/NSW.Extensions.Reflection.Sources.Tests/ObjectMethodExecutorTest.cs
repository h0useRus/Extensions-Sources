using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NSW.Extensions.Internal
{
    [TestFixture]
    public class ObjectMethodExecutorTest
    {
        private TestObject _targetObject = new TestObject();
        private TypeInfo targetTypeInfo = typeof(TestObject).GetTypeInfo();

        [Test]
        public void ExecuteValueMethod()
        {
            var executor = GetExecutorForMethod("ValueMethod");
            var result = executor.Execute(
                _targetObject,
                new object[] { 10, 20 });
            Assert.False(executor.IsMethodAsync);
            Assert.AreEqual(30, (int)result);
        }

        [Test]
        public void ExecuteVoidValueMethod()
        {
            var executor = GetExecutorForMethod("VoidValueMethod");
            var result = executor.Execute(
                _targetObject,
                new object[] { 10 });
            Assert.False(executor.IsMethodAsync);
            Assert.Null(result);
        }

        [Test]
        public void ExecuteValueMethodWithReturnType()
        {
            var executor = GetExecutorForMethod("ValueMethodWithReturnType");
            var result = executor.Execute(
                _targetObject,
                new object[] { 10 });
            Assert.IsInstanceOf<TestObject>(result);
            Assert.False(executor.IsMethodAsync);
            Assert.AreEqual("Hello", ((TestObject)result).value);
        }

        [Test]
        public void ExecuteValueMethodUpdateValue()
        {
            var executor = GetExecutorForMethod("ValueMethodUpdateValue");
            var parameter = new TestObject();
            var result = executor.Execute(
                _targetObject,
                new object[] { parameter });
            Assert.IsInstanceOf<TestObject>(result);
            Assert.False(executor.IsMethodAsync);
            Assert.AreEqual("HelloWorld", ((TestObject)result).value);
        }

        [Test]
        public void ExecuteValueMethodWithReturnTypeThrowsException()
        {
            var executor = GetExecutorForMethod("ValueMethodWithReturnTypeThrowsException");
            var parameter = new TestObject();
            Assert.False(executor.IsMethodAsync);
            Assert.Throws<NotImplementedException>(
                        () => executor.Execute(
                            _targetObject,
                            new object[] { parameter }));
        }

        [Test]
        public async Task ExecuteValueMethodAsync()
        {
            var executor = GetExecutorForMethod("ValueMethodAsync");
            var result = await executor.ExecuteAsync(
                _targetObject,
                new object[] { 10, 20 });
            Assert.True(executor.IsMethodAsync);
            Assert.AreEqual(30, (int)result);
        }

        [Test]
        public async Task ExecuteValueMethodWithReturnTypeAsync()
        {
            var executor = GetExecutorForMethod("ValueMethodWithReturnTypeAsync");
            var result = await executor.ExecuteAsync(
                _targetObject,
                new object[] { 10 });
            Assert.IsInstanceOf<TestObject>(result);
            Assert.True(executor.IsMethodAsync);
            Assert.AreEqual("Hello", ((TestObject)result).value);
        }

        [Test]
        public async Task ExecuteValueMethodUpdateValueAsync()
        {
            var executor = GetExecutorForMethod("ValueMethodUpdateValueAsync");
            var parameter = new TestObject();
            var result = await executor.ExecuteAsync(
                _targetObject,
                new object[] { parameter });
            Assert.IsInstanceOf<TestObject>(result);
            Assert.True(executor.IsMethodAsync);
            Assert.AreEqual("HelloWorld", ((TestObject)result).value);
        }

        [Test]
        public void ExecuteValueMethodWithReturnTypeThrowsExceptionAsync()
        {
            var executor = GetExecutorForMethod("ValueMethodWithReturnTypeThrowsExceptionAsync");
            var parameter = new TestObject();
            Assert.True(executor.IsMethodAsync);
            Assert.ThrowsAsync<NotImplementedException>(
                    async () => await executor.ExecuteAsync(
                            _targetObject,
                            new object[] { parameter }));
        }

        [Test]
        public void ExecuteValueMethodWithReturnVoidThrowsExceptionAsync()
        {
            var executor = GetExecutorForMethod("ValueMethodWithReturnVoidThrowsExceptionAsync");
            var parameter = new TestObject();
            Assert.True(executor.IsMethodAsync);
            Assert.ThrowsAsync<NotImplementedException>(
                    async () => await executor.ExecuteAsync(
                            _targetObject,
                            new object[] { parameter }));
        }

        [Test]
        public void GetDefaultValueForParameters_ReturnsSuppliedValues()
        {
            var suppliedDefaultValues = new object[] { 123, "test value" };
            var executor = GetExecutorForMethod("MethodWithMultipleParameters", suppliedDefaultValues);
            Assert.AreEqual(suppliedDefaultValues[0], executor.GetDefaultValueForParameter(0));
            Assert.AreEqual(suppliedDefaultValues[1], executor.GetDefaultValueForParameter(1));
            Assert.Throws<ArgumentOutOfRangeException>(() => executor.GetDefaultValueForParameter(2));
        }

        [Test]
        public void GetDefaultValueForParameters_ThrowsIfNoneWereSupplied()
        {
            var executor = GetExecutorForMethod("MethodWithMultipleParameters");
            Assert.Throws<InvalidOperationException>(() => executor.GetDefaultValueForParameter(0));
        }

        [Test]
        public async Task TargetMethodReturningCustomAwaitableOfReferenceType_CanInvokeViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableOfReferenceTypeAsync");

            // Act
            var result = await (TestAwaitable<TestObject>)executor.Execute(_targetObject, new object[] { "Hello", 123 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(TestObject), executor.AsyncResultType);
            Assert.NotNull(result);
            Assert.AreEqual("Hello 123", result.value);
        }

        [Test]
        public async Task TargetMethodReturningCustomAwaitableOfValueType_CanInvokeViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableOfValueTypeAsync");

            // Act
            var result = await (TestAwaitable<int>)executor.Execute(_targetObject, new object[] { 123, 456 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(int), executor.AsyncResultType);
            Assert.AreEqual(579, result);
        }

        [Test]
        public async Task TargetMethodReturningCustomAwaitableOfReferenceType_CanInvokeViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableOfReferenceTypeAsync");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { "Hello", 123 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(TestObject), executor.AsyncResultType);
            Assert.NotNull(result);
            Assert.IsInstanceOf<TestObject>(result);
            Assert.AreEqual("Hello 123", ((TestObject)result).value);
        }

        [Test]
        public async Task TargetMethodReturningCustomAwaitableOfValueType_CanInvokeViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableOfValueTypeAsync");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { 123, 456 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(int), executor.AsyncResultType);
            Assert.NotNull(result);
            Assert.IsInstanceOf<int>(result);
            Assert.AreEqual(579, (int)result);
        }

        [Test]
        public async Task TargetMethodReturningAwaitableOfVoidType_CanInvokeViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("VoidValueMethodAsync");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { 123 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(void), executor.AsyncResultType);
            Assert.Null(result);
        }
        
        [Test]
        public async Task TargetMethodReturningAwaitableWithICriticalNotifyCompletion_UsesUnsafeOnCompleted()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableWithICriticalNotifyCompletion");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[0]);

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("Used UnsafeOnCompleted", (string)result);
        }

        [Test]
        public async Task TargetMethodReturningAwaitableWithoutICriticalNotifyCompletion_UsesOnCompleted()
        {
            // Arrange
            var executor = GetExecutorForMethod("CustomAwaitableWithoutICriticalNotifyCompletion");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[0]);

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("Used OnCompleted", (string)result);
        }
        
        [Test]
        public async Task TargetMethodReturningValueTaskOfValueType_CanBeInvokedViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("ValueTaskOfValueType");

            // Act
            var result = await (ValueTask<int>)executor.Execute(_targetObject, new object[] { 123 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(int), executor.AsyncResultType);
            Assert.AreEqual(123, result);
        }

        [Test]
        public async Task TargetMethodReturningValueTaskOfReferenceType_CanBeInvokedViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("ValueTaskOfReferenceType");

            // Act
            var result = await (ValueTask<string>)executor.Execute(_targetObject, new object[] { "test result" });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("test result", result);
        }

        [Test]
        public async Task TargetMethodReturningValueTaskOfValueType_CanBeInvokedViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("ValueTaskOfValueType");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { 123 });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(int), executor.AsyncResultType);
            Assert.NotNull(result);
            Assert.AreEqual(123, (int)result);
        }

        [Test]
        public async Task TargetMethodReturningValueTaskOfReferenceType_CanBeInvokedViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("ValueTaskOfReferenceType");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { "test result" });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("test result", result);
        }

        [Test]
        public async Task TargetMethodReturningFSharpAsync_CanBeInvokedViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("FSharpAsyncMethod");

            // Act
            var fsharpAsync = (FSharpAsync<string>)executor.Execute(_targetObject, new object[] { "test result" });
            var result = await FSharpAsync.StartAsTask(fsharpAsync,
                FSharpOption<TaskCreationOptions>.None,
                FSharpOption<CancellationToken>.None);

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("test result", result);
        }

        [Test]
        public void TargetMethodReturningFailingFSharpAsync_CanBeInvokedViaExecute()
        {
            // Arrange
            var executor = GetExecutorForMethod("FSharpAsyncFailureMethod");

            // Act
            var fsharpAsync = (FSharpAsync<string>)executor.Execute(_targetObject, new object[] { "test result" });
            var resultTask = FSharpAsync.StartAsTask(fsharpAsync,
                FSharpOption<TaskCreationOptions>.None,
                FSharpOption<CancellationToken>.None);

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);

            var exception = Assert.ThrowsAsync<AggregateException>(async () => await resultTask);
            Assert.IsInstanceOf<InvalidOperationException>(exception!.InnerException);
            Assert.AreEqual("Test exception", exception.InnerException!.Message);
        }

        [Test]
        public async Task TargetMethodReturningFSharpAsync_CanBeInvokedViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("FSharpAsyncMethod");

            // Act
            var result = await executor.ExecuteAsync(_targetObject, new object[] { "test result" });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);
            Assert.AreEqual("test result", result);
        }

        [Test]
        public void TargetMethodReturningFailingFSharpAsync_CanBeInvokedViaExecuteAsync()
        {
            // Arrange
            var executor = GetExecutorForMethod("FSharpAsyncFailureMethod");

            // Act
            var resultTask = executor.ExecuteAsync(_targetObject, new object[] { "test result" });

            // Assert
            Assert.True(executor.IsMethodAsync);
            Assert.AreSame(typeof(string), executor.AsyncResultType);

            var exception = Assert.ThrowsAsync<AggregateException>(async () => await resultTask);
            Assert.IsInstanceOf<InvalidOperationException>(exception!.InnerException);
            Assert.AreEqual("Test exception", exception!.InnerException!.Message);
        }

        private ObjectMethodExecutor GetExecutorForMethod(string methodName)
        {
            var method = typeof(TestObject).GetMethod(methodName);
            return ObjectMethodExecutor.Create(method!, targetTypeInfo);
        }

        private ObjectMethodExecutor GetExecutorForMethod(string methodName, object[] parameterDefaultValues)
        {
            var method = typeof(TestObject).GetMethod(methodName);
            return ObjectMethodExecutor.Create(method!, targetTypeInfo, parameterDefaultValues);
        }

        public class TestObject
        {
            public string? value;
            public int ValueMethod(int i, int j) => i + j;

            public void VoidValueMethod(int i)
            {

            }

            public TestObject ValueMethodWithReturnType(int i) => new TestObject() { value = "Hello" };

            public TestObject ValueMethodWithReturnTypeThrowsException(TestObject i) => throw new NotImplementedException("Not Implemented Exception");

            public TestObject ValueMethodUpdateValue(TestObject parameter)
            {
                parameter.value = "HelloWorld";
                return parameter;
            }

            public Task<int> ValueMethodAsync(int i, int j) => Task.FromResult<int>(i + j);

            public async Task VoidValueMethodAsync(int i) => await ValueMethodAsync(3, 4);

            public Task<TestObject> ValueMethodWithReturnTypeAsync(int i) => Task.FromResult<TestObject>(new TestObject() { value = "Hello" });

            public async Task ValueMethodWithReturnVoidThrowsExceptionAsync(TestObject i)
            {
                await Task.CompletedTask;
                throw new NotImplementedException("Not Implemented Exception");
            }

            public async Task<TestObject> ValueMethodWithReturnTypeThrowsExceptionAsync(TestObject i)
            {
                await Task.CompletedTask;
                throw new NotImplementedException("Not Implemented Exception");
            }

            public Task<TestObject> ValueMethodUpdateValueAsync(TestObject parameter)
            {
                parameter.value = "HelloWorld";
                return Task.FromResult<TestObject>(parameter);
            }

            public TestAwaitable<TestObject> CustomAwaitableOfReferenceTypeAsync(
                string input1,
                int input2) =>
                new TestAwaitable<TestObject>(new TestObject
                {
                    value = $"{input1} {input2}"
                });

            public TestAwaitable<int> CustomAwaitableOfValueTypeAsync(
                int input1,
                int input2) =>
                new TestAwaitable<int>(input1 + input2);

            public TestAwaitableWithICriticalNotifyCompletion CustomAwaitableWithICriticalNotifyCompletion() => new TestAwaitableWithICriticalNotifyCompletion();

            public TestAwaitableWithoutICriticalNotifyCompletion CustomAwaitableWithoutICriticalNotifyCompletion() => new TestAwaitableWithoutICriticalNotifyCompletion();

            public ValueTask<int> ValueTaskOfValueType(int result) => new ValueTask<int>(result);

            public ValueTask<string> ValueTaskOfReferenceType(string result) => new ValueTask<string>(result);

            public void MethodWithMultipleParameters(int valueTypeParam, string referenceTypeParam)
            {
            }

            public FSharpAsync<string> FSharpAsyncMethod(string parameter) => FSharpAsync.AwaitTask(Task.FromResult(parameter));

            public FSharpAsync<string> FSharpAsyncFailureMethod(string parameter) =>
                FSharpAsync.AwaitTask(
                    Task.FromException<string>(new InvalidOperationException("Test exception")));
        }

        public class TestAwaitable<T>
        {
            private T _result;
            private bool _isCompleted;
            private List<Action> _onCompletedCallbacks = new List<Action>();

            public TestAwaitable(T result)
            {
                _result = result;

                // Simulate a brief delay before completion
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    Thread.Sleep(100);
                    SetCompleted();
                });
            }

            private void SetCompleted()
            {
                _isCompleted = true;

                foreach (var callback in _onCompletedCallbacks)
                {
                    callback();
                }
            }

            public TestAwaiter GetAwaiter()
            {
                return new TestAwaiter(this);
            }

            public struct TestAwaiter : INotifyCompletion
            {
                private TestAwaitable<T> _owner;

                public TestAwaiter(TestAwaitable<T> owner) : this()
                {
                    _owner = owner;
                }

                public bool IsCompleted => _owner._isCompleted;

                public void OnCompleted(Action continuation)
                {
                    if (_owner._isCompleted)
                    {
                        continuation();
                    }
                    else
                    {
                        _owner._onCompletedCallbacks.Add(continuation);
                    }
                }

                public T GetResult()
                {
                    return _owner._result;
                }
            }
        }

        public class TestAwaitableWithICriticalNotifyCompletion
        {
            public TestAwaiterWithICriticalNotifyCompletion GetAwaiter()
                => new TestAwaiterWithICriticalNotifyCompletion();
        }

        public class TestAwaitableWithoutICriticalNotifyCompletion
        {
            public TestAwaiterWithoutICriticalNotifyCompletion GetAwaiter()
                => new TestAwaiterWithoutICriticalNotifyCompletion();
        }

        public class TestAwaiterWithICriticalNotifyCompletion
            : CompletionTrackingAwaiterBase, ICriticalNotifyCompletion
        {
        }

        public class TestAwaiterWithoutICriticalNotifyCompletion
            : CompletionTrackingAwaiterBase, INotifyCompletion
        {
        }

        public class CompletionTrackingAwaiterBase
        {
            private string? _result;

            public bool IsCompleted { get; private set; }

            public string? GetResult() => _result;

            public void OnCompleted(Action continuation)
            {
                _result = "Used OnCompleted";
                IsCompleted = true;
                continuation();
            }

            public void UnsafeOnCompleted(Action continuation)
            {
                _result = "Used UnsafeOnCompleted";
                IsCompleted = true;
                continuation();
            }
        }
    }
}