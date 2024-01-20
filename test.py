import numpy as np
import matplotlib.pyplot as plt
import csv
#path = steps = np.random.normal(size=(3600, 2))
# Path generate
path = []
with open('path.csv', 'r') as f:
    rdr = csv.reader(f)
    for line in rdr:
        tup1 = []
        if ('Maze' in line[0]):
            tup1.append(int(line[0].split()[2])+1)
            tup1.append(int(line[0].split()[3])+1)
            path.append(tuple(tup1))
f.close
path = np.array(path)

# pos(n) = pos(n-1) + step(n)
for n in range(path.shape[0]-1):
    path[n+1] += path[n]

# Compact way to plot x and y: (3600,2) -> (2,3600) and the * expand along the first axis
plt.plot(*path.T)
plt.show()
