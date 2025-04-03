import logging
import os

if __name__ == "__main__":
    path = os.path.abspath(__file__)
    print(path)
    path = os.path.join(os.path.dirname(path), "log.txt")
    print(path)
    try:
        logging.basicConfig(filemode="w", filename=path, level=logging.DEBUG, format='%(asctime)s - %(levelname)s - %(message)s')
        logging.debug('This is a debug message')
        logging.info('This is an info message')
    except Exception as e:
        logging.error('Error occurred: %s', e)