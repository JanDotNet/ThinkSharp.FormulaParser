﻿using ThinkSharp.FormulaParsing.Ast.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkSharp.FormulaParsing
{
    public class FormulaParserResult<TResult>
    {
        private readonly TResult value = default(TResult);

        public static implicit operator TResult(FormulaParserResult<TResult> d) => d.Value;

        internal FormulaParserResult(Error error)
        {
            this.Error = error ?? throw new ArgumentNullException(nameof(error));
            this.value = default(TResult);
        }

        internal FormulaParserResult(TResult result)
        {
            this.value = result;
        }

        public bool Success => Error == null;

        public Error Error { get; } = null;

        public TResult Value
        {
            get
            {
                EnsureResultHasNoErrors();
                return value;
            }
        }

        private void EnsureResultHasNoErrors()
        {
            if (!this.Success)
            {
                throw new InvalidOperationException($"Unable to access {nameof(this.Value)} because result has errors:" +
                                                    Environment.NewLine + this.Error);
            }
        }

        public TResult HandleError(Func<Error, Exception> onError)
        {
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            if (this.Success)
            {            
                return Value;
            }
            else
            {
                throw onError(this.Error);
            }
        }

        public bool Handle(Action<TResult> onSuccess, Action<Error> onError)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            if (this.Success)
            {
                onSuccess(this.Value);
                return true;
            }
            else
            {
                onError(this.Error);
                return false;
            }
        }

        public bool Handle(Func<TResult, bool> onSuccess, Action<Error> onError)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            if (this.Success)
            {
                return onSuccess(this.Value);
            }
            else
            {
                onError(this.Error);
                return false;
            }
        }
    }
}
