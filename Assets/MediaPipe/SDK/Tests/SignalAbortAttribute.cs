using NUnit.Framework;
using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
public class SignalAbortAttribute : CategoryAttribute {}
