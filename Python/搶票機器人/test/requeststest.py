import requests
import json

web = requests.get(r"https://www.google.com/") #放在標頭傳送(公開)
web2 = requests.post(r"https://www.google.com/") #放在內容傳送(隱蔽)
print(web.url)
print(web2.url)
# print(web.json()) # 這個會報錯，因為google的網頁不是json格式