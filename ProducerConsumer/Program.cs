using System.Threading.Tasks.Dataflow;

async Task<int> ConsumeAsync(IReceivableSourceBlock<byte[]> sourceBlock)
{
	int bytesProcessed = 0;

	while (await sourceBlock.OutputAvailableAsync()) 
	{
		while (sourceBlock.TryReceive(out byte[] data))
		{
			bytesProcessed += data.Length;
		}
	}

	return bytesProcessed;
}


void  Produce(ITargetBlock<byte[]> targetBlock)
{
	Random rand = new Random();
	for (int i = 0; i < 100; ++i)
	{
		var buffer = new byte[1024];
		rand.NextBytes(buffer);
		targetBlock.Post(buffer);
	}

	targetBlock.Complete();
}


var buffer = new BufferBlock<byte[]>();

var consumertask =ConsumeAsync(buffer);
Produce(buffer);


Console.WriteLine($"Data processed {await consumertask}");

