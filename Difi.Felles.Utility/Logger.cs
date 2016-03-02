﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Difi.Felles.Utility
{
    public class Logger
    {
        private static Action<TraceEventType, Guid?, string, string> _logAction;

        public static TraceSource TraceSource { get; set; }

        public static void Initialize(GeneriskKlientkonfigurasjon konfigurasjon)
        {
            _logAction = konfigurasjon.Logger;
        }

        public static void Log(TraceEventType severity, string message, [CallerMemberName] string callerMember = null)
        {
            Log(severity, null, message, callerMember);
        }

        public static void Log(TraceEventType severity, Guid? conversationId, string message, [CallerMemberName] string callerMember = null)
        {
            if (_logAction == null)
                return;

            if (callerMember == null)
                callerMember = new StackFrame(1).GetMethod().Name;

            _logAction(severity, conversationId, callerMember, message);
        }

        public static Action<TraceEventType, Guid?, string, string> TraceLogger()
        {
            var traceSource = TraceSource;
            return (severity, koversasjonsId, caller, message) => { traceSource.TraceEvent(severity, 1, "[{0}, {1}] {2}", koversasjonsId.GetValueOrDefault(), caller, message); };
        }

        public static Action<TraceEventType, Guid?, string, string> ConsoleLogger()
        {
            return (severity, koversasjonsId, caller, message) => { Console.WriteLine("[{0}] {1}", caller, message); };
        }
    }
}