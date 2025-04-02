import os
import sys
import selenium
from selenium import webdriver

driver = webdriver.Chrome()
try:
    driver.get("https://www.google.com/")
    input()
finally:
    driver.quit()