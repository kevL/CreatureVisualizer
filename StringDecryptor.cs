using System;


namespace creaturevisualizer
{
	static class StringDecryptor
	{
		internal static string Decrypt(string st)
		{
			char[] array0;
			char[] array1 = (array0 = st.ToCharArray());

			int p1 = array1.Length;
			while (p1 != 0)
			{
				int p0 = p1 - 1;
				array1[p0] = (char)(array0[p0] - 5225);
				array1 = array0; // wtf.
				p1 = p0;
			}

			return String.Intern(new string(array1));
		}
	}
}
