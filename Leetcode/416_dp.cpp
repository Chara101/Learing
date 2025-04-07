#include <bits/stdc++.h>
using namespace std;

class Solution {
public:
    bool canPartition(vector<int>& nums) {
        int total = 0;
        for(int num : nums) total += num;
        if(total % 2 != 0) return false;
        total /= 2;
        vector<bool> dp(total + 1, false);
        dp[0] = true; //可構成0的只有0自己
        for(int num : nums){ //嘗試用每一個數構成子集
            for(int i = total; i >= num; i--){ //從目標值向下找可能構成total的值
                /*
                    如果本身已經可以構成則不變
                    如果本身可成為目標的子集則尋找剩下子集能不能被構成
                    每次都刷新一次total是否可構成
                */
                if(dp[i - num]) dp[i] = dp[i - num]; //(i - num)是代表有辦法構成i - num的數x，x+num = i，因為num必定存在
            }
        }
        return dp[total];
    }
};