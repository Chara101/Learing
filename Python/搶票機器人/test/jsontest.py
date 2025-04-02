import json
import os
if __name__ == "__main__":
    # 讀取 JSON 檔案
    path = os.path.dirname(__file__) # 取得當前檔案的路徑
    path = os.path.join(path, "test1.json") # 將路徑與檔名結合
    with open(path, "r") as file: #建立資料流
        #a = json.load(file) #進來時轉換成字典，且讀到結尾
        a = file.read() # 讀取 JSON 檔案，讀到結尾
        a = json.loads(a) # 將 JSON 字串轉換成字典
    print(a)
