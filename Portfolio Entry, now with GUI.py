from tkinter import *

root = Tk()
root.title("Quantum Numbers")

# input last electron config
InputLabel = Label(root, text ="Input last electron configuration of element (ex. 1s^2):")
InputLabel.grid(column = 0, row = 0)

input = Entry(root, width = 10)
input.grid(column = 1, row = 0) # NOTEE: for future coding with GUI's, ALWAYS PUT THE POSITION OF WIDGET IN ANOTHER LINE!!!

# list of orbital diagrams
p_list = [-1, 0, 1, -1, 0, 1]
d_list = [-2, -1, 0, 1, 2, -2, -1, 0, 1, 2]
f_list = [-3, -2, -1, 0, 1, 2, 3, -3, -2, -1, 0, 1, 2, 3]

output = Label(root) # placeholder for output
output.grid(column = 0, row = 2, columnspan=2)

# function does the process to find the 4 quantum numbers
def processquantum():
    # restriction
    possible = True
    # get the input
    config = input.get()

    slice_config = config[3:]
    last_electron_position = int(slice_config)

    # 1st quantum number
    n = config[0]

    # 2nd quantum number
    if config[1] == "s":
        l = 0
    elif config[1] == "p":
        l = 1
    elif config[1] == "d":
        l = 2
    elif config[1] == "f":
        l = 3

    # 3rd and 4th quantum number
    if config[1] == "s":
        if last_electron_position > 2: # restriction
            possible = False
            error = "the s orbital can only hold a maximum of 2 electrons!"
        else:
            ml = 0
            if last_electron_position == 1:
                ms = 1/2
            else:
                ms = -1/2
    elif config[1] == "p":
        if last_electron_position > 6: # restrictions
            possible = False
            error = "the p orbital can only hold a maximum of 6 electrons!"
        elif int(n) < 2:
            possible = False
            error = "the p orbital only begins at the 2nd energy level!"
        else:
            ml = p_list[int(config[-1]) - 1]
            if last_electron_position <= 3:
                ms = 1/2
            else:
                ms = -1/2
    elif config[1] == "d":
        if last_electron_position > 10: # restrictions
            possible = False
            error = "the d orbital can only hold a maximum of 10 electrons!"
        elif int(n) < 3:
            possible = False
            error = "the d orbital only begins at the 3rd energy level!"
        else:
            ml = d_list[int(config[-1]) - 1]
            if last_electron_position <= 5:
                ms = 1/2
            else:
                ms = -1/2
    elif config[1] == "f":
        if last_electron_position > 14: # restrictions
            possible = False
            error = "the f orbital can only hold a maximum of 14 electrons!"
        elif int(n) < 4:
            possible = False
            error = "the f orbital only begins at the 4th energy level!"
        else:
            ml = f_list[int(config[-1]) - 1]
            if last_electron_position <= 7:
                ms = 1/2
            else:
                ms = -1/2

    global output # call output variable from outside the function
    if possible == True:
        output.destroy()
        output = Label(root, text = f"4 Quantum numbers: ({n}, {l}, {ml}, {ms})")
        output.grid(column = 0, row = 2, columnspan=2)
    else:
        output.destroy()
        output = Label(root, text = error)
        output.grid(column = 0, row = 2, columnspan=2)

# process the function
button = Button(root, text="Process", command=processquantum)
button.grid(column=0, row=1, columnspan=2)

mainloop()