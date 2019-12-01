// Learn more about F# at http://fsharp.org

open System
open ILGPU
open ILGPU.Runtime

/// This is basically an "op_Implicit" operator, since F# doesn't support
/// implicit conversions out of the box.
let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

type Test () = class end 
    with static member MyKernel (index:Index) (dataView:ArrayView<int>) (constant:int) = 
            dataView.[index] <- (!> index) + constant

[<EntryPoint>]
let main argv =
    use context = new Context()
    for acceleratorId in Accelerator.Accelerators do
        use accelerator = Accelerator.Create(context,acceleratorId)
        Console.WriteLine("Performing operations on {0}",accelerator)
        let action= Action<Index,ArrayView<int>,int>(Test.MyKernel)
        let kernel = accelerator.LoadAutoGroupedStreamKernel<Index, ArrayView<int>, int>(action);
        use buffer = accelerator.Allocate<int>(1024)
        kernel.Invoke(!> buffer.Length,buffer.View,42)
        accelerator.Synchronize()
        let data = buffer.GetAsArray()
        for i = 0 to data.Length - 1 do
            if (data.[i] <> 42 + i) then
                Console.WriteLine("Error at element {0} | {1} found",i,data.[i])

    printfn "All done!"

    0 // return an integer exit code
