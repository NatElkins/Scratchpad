// -----------------------------------------------------------------------------
//                                    ILGPU
//                     Copyright (c) 2016-2019 Marcel Koester
//                                www.ilgpu.net
//
// File: ClosureExtensions.cs
//
// This file is part of ILGPU and is distributed under the University of
// Illinois Open Source License. See LICENSE.txt for details.
// -----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Reflection;
using ILGPU.Frontend;
using Microsoft.FSharp.Core;

namespace ILGPU.FSharp
{
    static class ClosureExtensions
    {
        public static MethodInfo GetClosureMethodInfo<T, TResult>(this FSharpFunc<T, TResult> action)
        {
            // FSharp closures are implemented in MSIL as an auto-generated class.
            // Extract the static kernel method that is wrapped in a non-static FSharp closure.
            // Disassemble the closure and extract the first call (which should be to the static kernel method)
            var typeInfo = (TypeInfo)action.GetType();
            var closureMethodInfo = typeInfo.GetDeclaredMethod(nameof(FSharpFunc<T, TResult>.Invoke));
            var disassembler = new Disassembler(closureMethodInfo, null);
            var disassembledMethod = disassembler.Disassemble();
            var callInstruction = disassembledMethod.Instructions.First(x => x.InstructionType == ILInstructionType.Call);

            if (callInstruction.Argument is MethodInfo kernelMethodInfo)
            {
                return kernelMethodInfo;
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
