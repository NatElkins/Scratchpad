// -----------------------------------------------------------------------------
//                                    ILGPU
//                     Copyright (c) 2016-2019 Marcel Koester
//                                www.ilgpu.net
//
// File: FSharpKernelLoaders.tt/FSharpKernelLoaders.cs
//
// This file is part of ILGPU and is distributed under the University of
// Illinois Open Source License. See LICENSE.txt for details.
// -----------------------------------------------------------------------------


using System;
using ILGPU.FSharp;
using Microsoft.FSharp.Core;

namespace ILGPU.Runtime
{
    /// <summary>
    /// Contains extensions for convenient kernel loading of default kernels.
    /// </summary>
    public static class FSharpKernelLoaders
    {
        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>> LoadKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, Unit>> LoadStreamKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>> LoadKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, Unit>> LoadStreamKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>> LoadImplicitlyGroupedKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, Unit>> LoadImplicitlyGroupedStreamKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>> LoadAutoGroupedKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, Unit>>> LoadAutoGroupedKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, Unit>> LoadAutoGroupedStreamKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, Unit>> LoadAutoGroupedStreamKernel<TIndex, T1>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, Unit>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>> LoadKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> LoadStreamKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>> LoadKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> LoadStreamKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>> LoadAutoGroupedKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>>> LoadAutoGroupedKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, Unit>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>> LoadKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> LoadStreamKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>> LoadKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> LoadStreamKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, Unit>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>> LoadKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>> LoadKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, Unit>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, Unit>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, Unit>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, Unit>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, Unit>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, Unit>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, Unit>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, Unit>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, Unit>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, Unit>>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>> LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
                action.GetClosureMethodInfo(),
                specialization);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="specialization">The kernel specialization.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernels will be launched with a group size
        /// of the current warp size of the accelerator.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> LoadStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            KernelSpecialization specialization)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            var baseKernel = accelerator.LoadKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action, specialization);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>> LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadImplicitlyGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
                action.GetClosureMethodInfo(),
                customGroupSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// group size.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="customGroupSize">The custom group size to use.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        /// <remarks>
        /// Note that implictly-grouped kernel will be launched with the given
        /// group size.
        /// </remarks>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> LoadImplicitlyGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            int customGroupSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            var baseKernel = accelerator.LoadImplicitlyGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action, customGroupSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(action.GetClosureMethodInfo());
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that
        /// can receive arbitrary accelerator streams (first parameter).
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<AcceleratorStream, FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>>> LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            var kernel = accelerator.LoadAutoGroupedKernel<Action<AcceleratorStream, TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>(
                action.GetClosureMethodInfo(),
                out groupSize,
                out minGridSize);
            return new FSharpKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(kernel);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(action);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

        /// <summary>
        /// Loads the given kernel and returns a launcher delegate that uses the default accelerator stream.
        /// </summary>
        /// <typeparam name="TIndex">The index type.</typeparam>
        /// <typeparam name="T1">Parameter type of parameter 1.</typeparam>
        /// <typeparam name="T2">Parameter type of parameter 2.</typeparam>
        /// <typeparam name="T3">Parameter type of parameter 3.</typeparam>
        /// <typeparam name="T4">Parameter type of parameter 4.</typeparam>
        /// <typeparam name="T5">Parameter type of parameter 5.</typeparam>
        /// <typeparam name="T6">Parameter type of parameter 6.</typeparam>
        /// <typeparam name="T7">Parameter type of parameter 7.</typeparam>
        /// <typeparam name="T8">Parameter type of parameter 8.</typeparam>
        /// <typeparam name="T9">Parameter type of parameter 9.</typeparam>
        /// <typeparam name="T10">Parameter type of parameter 10.</typeparam>
        /// <typeparam name="T11">Parameter type of parameter 11.</typeparam>
        /// <typeparam name="T12">Parameter type of parameter 12.</typeparam>
        /// <typeparam name="T13">Parameter type of parameter 13.</typeparam>
        /// <typeparam name="T14">Parameter type of parameter 14.</typeparam>

        /// <param name="accelerator">The current accelerator.</param>
        /// <param name="action">The action to compile into a kernel.</param>
        /// <param name="groupSize">The estimated group size to gain maximum occupancy on this device.</param>
        /// <param name="minGridSize">The minimum grid size to gain maximum occupancy on this device.</param>
        /// <returns>The loaded kernel-launcher delegate.</returns>
        public static FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> LoadAutoGroupedStreamKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Accelerator accelerator,
            FSharpFunc<TIndex, FSharpFunc<T1, FSharpFunc<T2, FSharpFunc<T3, FSharpFunc<T4, FSharpFunc<T5, FSharpFunc<T6, FSharpFunc<T7, FSharpFunc<T8, FSharpFunc<T9, FSharpFunc<T10, FSharpFunc<T11, FSharpFunc<T12, FSharpFunc<T13, FSharpFunc<T14, Unit>>>>>>>>>>>>>>> action,
            out int groupSize,
            out int minGridSize)
            where TIndex : struct, IIndex
            where T1 : struct where T2 : struct where T3 : struct where T4 : struct where T5 : struct where T6 : struct where T7 : struct where T8 : struct where T9 : struct where T10 : struct where T11 : struct where T12 : struct where T13 : struct where T14 : struct
        {
            var baseKernel = accelerator.LoadAutoGroupedKernel<TIndex, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
                action,
                out groupSize,
                out minGridSize);
            return baseKernel.Invoke(accelerator.DefaultStream);
        }

    }
}