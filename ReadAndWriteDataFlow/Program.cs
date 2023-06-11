#region Buffer Block  Sync
using System.Threading.Tasks.Dataflow;

BufferBlock<int> block = new BufferBlock<int>();

for (int i = 0; i < 3; i++)
{
  block.Post(i);
}

// poll data
for (int i = 0; i < 3; i++)
{
	block.TryReceive(out int value);
	Console.WriteLine(value);
	
}




BufferBlock<int> bufferBlock = new BufferBlock<int>();
var post01 = Task.Run(() =>
{
	bufferBlock.Post(0);
	bufferBlock.Post(1);
});


var receive = Task.Run(() =>
{
	for (int i = 0; i < 3; i++)
	{
		Console.WriteLine(bufferBlock.Receive());
	}
});
var post2 = Task.Run(() =>
{
	bufferBlock.Post(2);
});

await Task.WhenAll(post01, receive, post2);

#endregion


#region Writing block Async

Console.WriteLine("Writing block Async");

BufferBlock<int> bufferBlockAsync = new BufferBlock<int>();
for (int i = 0; i < 3; i++)
{
	await bufferBlockAsync.SendAsync(i);
}



for (int i = 0; i < 3; i++)
{
	Console.WriteLine(await bufferBlockAsync.ReceiveAsync());
}
#endregion