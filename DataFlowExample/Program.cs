

using System.Threading.Tasks.Dataflow;

#region Buffer block
var bufferBlock = new BufferBlock<int>();

for (int i = 0; i < 3; i++)
{
	bufferBlock.Post(i);
}



for (int i = 0; i < 3; i++)
{
	Console.WriteLine(bufferBlock.Receive());
}

#endregion

#region BroadCast Block

var broadcastBlock = new BroadcastBlock<double>(null);

broadcastBlock.Post(Math.PI);

for (int i = 0; i < 3; i++)
{
	Console.WriteLine($"{broadcastBlock.Receive()}");
}
#endregion



#region WriteOnceBlock

var writeOnceBlock = new WriteOnceBlock<string>(null);

// Post several messages to the block in parallel. The first
// message to be received is written to the block.
// Subsequent messages are discarded.
Parallel.Invoke(
   () => writeOnceBlock.Post("Message 1"),
   () => writeOnceBlock.Post("Message 2"),
   () => writeOnceBlock.Post("Message 3"));

// Receive the message from the block.
Console.WriteLine(writeOnceBlock.Receive());

Console.WriteLine(writeOnceBlock.Receive());

#endregion


#region Action Block <T>

Console.WriteLine("ActionBloc<T> -- start");

var actionblock = new ActionBlock<int>(n => Console.WriteLine(n));

for (int i = 0; i < 3; i++)
{
	actionblock.Post(i * 10);
}

actionblock.Complete();

await actionblock.Completion;



Console.WriteLine("ActionBlock<T> -- end");


#endregion


#region Action Block  Task <T>

Console.WriteLine("ActionBlock<Func<Tin,Task>> -- start");
Func<int, Task> powerFuncTask = async (i) =>
{
	await Task.Delay(2 * 1000);
	Console.WriteLine((int)Math.Pow(i, 2));
};

var actionBlockTask = new ActionBlock<int>(powerFuncTask);

for (int i = 1; i < 4; i++)
{
	actionBlockTask.Post(i);
}
Console.WriteLine("ActionBlock<Func<Tin,Task>> -- end");

actionBlockTask.Complete();
await actionBlockTask.Completion;
#endregion



#region Transform Block <Tin, Tout>
Console.WriteLine("Transform Block <Tin, Tout> -- Start");


TransformBlock<int, double> TfBlock = new TransformBlock<int, double>(a=>Math.Pow(a, 2));

TfBlock.Post(10);

TfBlock.Post(20);

TfBlock.Post(30);

for (int i = 0; i < 3; i++)
{
	Console.WriteLine(TfBlock.Receive());
}



TfBlock.Post(40);
Console.WriteLine(TfBlock.Receive());

TfBlock.Complete();



Console.WriteLine("Transform Block <Tin, Tout> -- End");

#endregion Transform Block <Tin, Tout>