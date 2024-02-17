namespace Ria.Services.Payment.Denomination;

public static class Main
{
    private static readonly int[] Cartridges = { 10, 50, 100 };

    private static readonly int[] Payments = { 30, 50, 60, 80, 140, 230, 370, 610, 980 };

    private static readonly List<Dictionary<int, int>> Denominations = new();

    public static void Run()
    {
        foreach (var payment in Payments)
        {
            Denominations.Clear();
            
            Console.WriteLine();
            Console.WriteLine($"Denomination payment for {payment} euros.");
            
            GetDenominations(payment, Cartridges, new Dictionary<int, int>());

            foreach (var denomination in Denominations)
            {
                Console.WriteLine();
                foreach (var item in denomination)
                    Console.WriteLine($"{item.Value} x {item.Key}");
            }
        }
    }

    private static void GetDenominations(int remaining, int[] cartridges, Dictionary<int, int> denomination)
    { 
        for (int i = cartridges.Length - 1; i >= 0; i--)
        {
            int mod = remaining % cartridges[i];
            int div = remaining / cartridges[i];

            if (mod == remaining) continue;

            while (div > 0)
            {
                denomination[cartridges[i]] = div;

                var newRemaining = remaining - cartridges[i] * div; 
                if (newRemaining == 0)
                {
                    Denominations.Add(new Dictionary<int, int>(denomination));
                    denomination.Remove(cartridges[i]);
                    
                    if (i == 0) break;
                    
                    div--;
                    continue;
                }

                GetDenominations(newRemaining, cartridges.Take(i).ToArray(), new Dictionary<int, int>(denomination));

                if (i == 0) break;
                denomination.Remove(cartridges[i]);
                div--;
            }
        }
    }
}