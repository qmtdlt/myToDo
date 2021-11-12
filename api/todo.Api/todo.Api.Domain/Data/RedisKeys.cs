namespace todo.Api.Domain.Data
{
    public class RedisKeys
    {
        public const string todoList = "todoList";

        public int LengthOfLongestSubstring(string s)
        {
            int[] last = new int[128];
            for (int i = 0; i < 128; i++)
            {
                last[i] = -1;
            }


            int res = 0;
            int start = 0; // 窗口开始位置
            for (int i = 0; i < s.Length; i++)
            {
                int index = s[i];
                start = Math.Max(start, last[index] + 1);
                res = Math.Max(res, i - start + 1);
                last[index] = i;
            }

            return res;
        }
    }
}
