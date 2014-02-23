using System;
using System.Linq;
using Coherent.UI.Binding;

[CoherentType]
public class MyMath {
	
	[CoherentProperty]
	public double Sum(double[] numbers)
	{
		return numbers.Sum();
	}
	
	[CoherentProperty]
	public double Average(double[] numbers)
	{
		return numbers.Average();
	}
}
