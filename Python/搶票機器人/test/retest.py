import re

s = "what is http"
request = re.compile(r"http")
if request.search(s):
    print("yes")