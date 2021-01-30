using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace PaliBot.Numerics
{
    public static class Vector3Extensions
    {
        public static Vector3 Normalized(this Vector3 vector)
        {
            return Vector3.Normalize(vector);
        }

        public static float Dot(this Vector3 thisVector, Vector3 vector)
        {
            return Vector3.Dot(thisVector, vector);
        }

		public static Vector3 ToVector3(this IEnumerable<float> input)
		{
            if (input == null)
            {
                throw new Exception("Cannot convert null into Vector3");
            }

			var values = input.ToArray();
			if (values.Length != 3)
			{
				throw new Exception("Vector3 requires exactly three values (x,y,z)");
			}

			return new Vector3(values[0], values[1], values[2]);
		}
	}
}
