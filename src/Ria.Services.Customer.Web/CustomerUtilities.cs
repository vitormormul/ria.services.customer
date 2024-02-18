namespace Ria.Services.Customer.Web;

public static class CustomerUtilities
{
    
    public static void Sort(this Customer[] arr, int left = -1, int right = -1)
    {
        if (left == -1) left = 0;
        if (right == -1) right = arr.Length - 1;
        
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
 
            Sort(arr, left, pivot - 1);
            Sort(arr, pivot + 1, right);
        }
    }
 
    private static int Partition(Customer[] arr, int left, int right)
    {
        var pivot = arr[right];
        int i = left - 1;
 
        for (int j = left; j < right; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
 
        var temp1 = arr[i + 1];
        arr[i + 1] = arr[right];
        arr[right] = temp1;
 
        return i + 1;
    }
}