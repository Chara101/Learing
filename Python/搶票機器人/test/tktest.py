import tkinter as tk

window = tk.Tk()
window.title("Chara Bot")
window.geometry("400x300")
window.resizable(False, False)
test = tk.Button(window, text="Test", command=lambda: print("Test button clicked!"))
test.pack(side = tk.BOTTOM)
try:
    window.mainloop()
except:
    print("Error: Mainloop failed to run.")




    