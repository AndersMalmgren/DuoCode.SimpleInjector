// Partial type definitions for qunit.js
// Project: http://qunitjs.com/

using System;
using DuoCode.Runtime;

#pragma warning disable 0626 // disable: method is marked external and has no attributes on it

namespace QUnitJs
{
    [Js(Extern = true)]
    public static class QUnit
    {
        // http://api.qunitjs.com/category/test/

        public static extern void module(string name, dynamic hooks);
        public static extern void skip(string testName, Action action);
        public static extern void test(string testName, Action action);

        // http://api.qunitjs.com/category/assert/

        public static extern void deepEqual(object actual, object expected);
        public static extern void deepEqual(object actual, object expected, string message);

        public static extern void equal(object actual, object expected);
        public static extern void equal(object actual, object expected, string message);

        [Js(Name = "equal", OmitGenericArgs = true)]
        public static extern void equalT<T>(T actual, T expected) where T : struct; // prevents boxing for enums and value-types

        public static extern void expect(int amount);

        public static extern void notDeepEqual(object actual, object expected);
        public static extern void notDeepEqual(object actual, object expected, string message);

        public static extern void notEqual(object actual, object expected);
        public static extern void notEqual(object actual, object expected, string message);

        public static extern void notPropEqual(object actual, object expected);
        public static extern void notPropEqual(object actual, object expected, string message);

        public static extern void notStrictEqual(object actual, object expected);
        public static extern void notStrictEqual(object actual, object expected, string message);

        public static extern void ok(bool condition);
        public static extern void ok(bool condition, string message);

        public static extern void propEqual(object actual, object expected);
        public static extern void propEqual(object actual, object expected, string message);

        public static extern void push(bool result, object actual, object expected, string message);

        public static extern void strictEqual(object actual, object expected);
        public static extern void strictEqual(object actual, object expected, string message);

        public static extern void throws(Action block);
        public static extern void throws(Action block, Func<object, bool> expected);
        public static extern void throws(Action block, Func<object, bool> expected, string message);
    }

    public static class QUnitUtils
    {
        public static void Throws<T>(Action block)
          where T : Exception
        {
            QUnit.throws(block, (error) => typeof(T).IsAssignableFrom(error.GetType()));
        }
    }
}
