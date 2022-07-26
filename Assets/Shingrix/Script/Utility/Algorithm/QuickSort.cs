using System.Collections;
using System.Collections.Generic;

namespace Hsinpa.Algorithm
{
    public class QuickSort
    {

        public static T[] Sort<T>(T[] array, int leftIndex, int rightIndex) where T: ISort {
            var i = leftIndex;
            var j = rightIndex;
            var pivot = array[leftIndex].Value;

            while (i <= j)
            {
                while (array[i].Value < pivot)
                {
                    i++;
                }

                while (array[j].Value > pivot)
                {
                    j--;
                }

                if (array[i].Value <= array[j].Value)
                {
                    T temp = array[i];

                    array[i] = array[j];
                    array[i].SetIndex(j);

                    array[j] = temp;
                    array[j].SetIndex(i);

                    i++;
                    j--;
                }
            }

            if (array[leftIndex].Value < array[j].Value)
                Sort(array, leftIndex, j);
            if (array[i].Value < array[rightIndex].Value)
                Sort(array, i, rightIndex);
            return array;
        }

    }
}