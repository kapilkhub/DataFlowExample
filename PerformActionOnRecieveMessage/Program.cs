using System.Threading.Tasks.Dataflow;

static int CountBytes(string path)
{
	byte[] buffer = new byte[1024];
	int totalZeroBytesRead = 0;
	using (var fileStream = File.OpenRead(path))
	{
		int bytesRead = 0;
		do
		{
			bytesRead = fileStream.Read(buffer, 0, buffer.Length);
			totalZeroBytesRead += buffer.Count(b => b == 0);
		} while (bytesRead > 0);
	}

	return totalZeroBytesRead;
}



string tempFile = Path.GetTempFileName();

// Write random data to the temporary file.
using (var fileStream = File.OpenWrite(tempFile))
{
	Random rand = new Random();
	byte[] buffer = new byte[1024];
	for (int i = 0; i < 512; i++)
	{
		rand.NextBytes(buffer);
		fileStream.Write(buffer, 0, buffer.Length);
	}
}



var printResult = new ActionBlock<int>(zeroBytesRead =>
{
	Console.WriteLine("{0} contains {1} zero bytes.",
	   Path.GetFileName(tempFile), zeroBytesRead);
});



var countBytesBlock = new TransformBlock<string,int>(CountBytes);

countBytesBlock.LinkTo(printResult);

 countBytesBlock.Completion.ContinueWith(a => printResult.Complete());


countBytesBlock.Post(tempFile);


countBytesBlock.Complete();

await printResult.Completion;

File.Delete(tempFile);