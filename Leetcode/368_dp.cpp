#include <bits/stdc++.h>
using namespace std;

class Solution {
    public:
        vector<int> largestDivisibleSubset(vector<int>& nums) {
            sort(nums.begin(), nums.end());
            int n = nums.size();
            int maximum = 0;
            int pointer = 0;
            int len = 0;
            vector<int> result;
            vector<int> dp(n, 0);
            vector<int> pre(n, -1);
            for(int i = 0; i < n; i++){
                int count = 1;
                if(i == 0){
                    dp[i] = 1;
                }
                else{
                    int record = -1; //最大上一位的index
                    int temp = 0;
                    for(int j = i - 1; j >= 0; j--){
                        if(nums[i] % nums[j] == 0){
                            if(dp[j] == -1){
                                record = j;
                                temp = 0;
                            }
                            else if(dp[j] > temp){
                                record = j;
                                temp = dp[record];
                            }
                            //break;
                        }
                    }
                    // if(record != -1){
                    //     count += dp[record];
                    //     pre[i] = record;
                    // }
                    count += temp;
                    pre[i] = record;
                    dp[i] = count;
                }
                if(dp[i] > maximum){
                    pointer = i;
                    maximum = dp[i];
                }
            }
            int temp = pointer;
            while(temp != -1){
                result.push_back(nums[temp]);
                temp = pre[temp];
            }
            sort(result.begin(), result.end());
            //sort(result.begin(), result.end(), greater<int>());
            return result;
        }
    };