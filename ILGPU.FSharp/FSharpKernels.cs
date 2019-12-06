// -----------------------------------------------------------------------------
//                                    ILGPU
//                     Copyright (c) 2016-2019 Marcel Koester
//                                www.ilgpu.net
//
// File: FSharpKernels.tt/FSharpKernels.cs
//
// This file is part of ILGPU and is distributed under the University of
// Illinois Open Source License. See LICENSE.txt for details.
// -----------------------------------------------------------------------------


using System;
using Microsoft.FSharp.Core;

namespace ILGPU.Runtime
{
    static class FSharpKernel
    {
        public static readonly Unit Unit = MakeUnit();

        private static Unit MakeUnit()
        {
            // Using reflection because ctor is internal
            return (Unit)Activator.CreateInstance(typeof(Unit), true);
        }
    }

    sealed class FSharpKernel<TIndex, T1> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>>
        where TIndex : struct, IIndex
        where T1 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1) => {
                    kernel(stream, index, param1);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, Unit>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, Unit>>(index =>
                new Converter<T1, Unit>(param1 =>
                _kernel(stream, index, param1)
                ));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2) => {
                    kernel(stream, index, param1, param2);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>(index =>
                new Converter<T1, FSharpFunc<T2, Unit>>(param1 =>
                new Converter<T2, Unit>(param2 =>
                _kernel(stream, index, param1, param2)
                )));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3) => {
                    kernel(stream, index, param1, param2, param3);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, Unit>>(param2 =>
                new Converter<T3, Unit>(param3 =>
                _kernel(stream, index, param1, param2, param3)
                ))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4) => {
                    kernel(stream, index, param1, param2, param3, param4);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, Unit>>(param3 =>
                new Converter<T4, Unit>(param4 =>
                _kernel(stream, index, param1, param2, param3, param4)
                )))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5) => {
                    kernel(stream, index, param1, param2, param3, param4, param5);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, Unit>>(param4 =>
                new Converter<T5, Unit>(param5 =>
                _kernel(stream, index, param1, param2, param3, param4, param5)
                ))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, Unit>>(param5 =>
                new Converter<T6, Unit>(param6 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6)
                )))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, Unit>>(param6 =>
                new Converter<T7, Unit>(param7 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7)
                ))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, Unit>>(param7 =>
                new Converter<T8, Unit>(param8 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8)
                )))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, Unit>>(param8 =>
                new Converter<T9, Unit>(param9 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9)
                ))))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>(param8 =>
                new Converter<T9, FSharpFunc<T10, Unit>>(param9 =>
                new Converter<T10, Unit>(param10 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10)
                )))))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>(param8 =>
                new Converter<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>(param9 =>
                new Converter<T10, FSharpFunc<T11, Unit>>(param10 =>
                new Converter<T11, Unit>(param11 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11)
                ))))))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>(param8 =>
                new Converter<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>(param9 =>
                new Converter<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>(param10 =>
                new Converter<T11, FSharpFunc<T12, Unit>>(param11 =>
                new Converter<T12, Unit>(param12 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12)
                )))))))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>(param8 =>
                new Converter<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>(param9 =>
                new Converter<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>(param10 =>
                new Converter<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>(param11 =>
                new Converter<T12, FSharpFunc<T13, Unit>>(param12 =>
                new Converter<T13, Unit>(param13 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13)
                ))))))))))))));
        }
    }

    sealed class FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>>
        where TIndex : struct, IIndex
        where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
    {
        private readonly Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit> _kernel;

        public FSharpKernel(Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> kernel)
        {
            _kernel = new Func<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Unit>(
                (AcceleratorStream stream, TIndex index, T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9, T10 param10, T11 param11, T12 param12, T13 param13, T14 param14) => {
                    kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14);
                    return FSharpKernel.Unit;
                });;
        }

        public override FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> Invoke(AcceleratorStream stream)
        {
            return new Converter<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>(index =>
                new Converter<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>(param1 =>
                new Converter<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>(param2 =>
                new Converter<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>(param3 =>
                new Converter<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>(param4 =>
                new Converter<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>(param5 =>
                new Converter<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>(param6 =>
                new Converter<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>(param7 =>
                new Converter<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>(param8 =>
                new Converter<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>(param9 =>
                new Converter<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>(param10 =>
                new Converter<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>(param11 =>
                new Converter<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>(param12 =>
                new Converter<T13, FSharpFunc<T14, Unit>>(param13 =>
                new Converter<T14, Unit>(param14 =>
                _kernel(stream, index, param1, param2, param3, param4, param5, param6, param7, param8, param9, param10, param11, param12, param13, param14)
                )))))))))))))));
        }
    }

}