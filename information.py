import math
while 1:
	positive = float(input("Positive: "))
	negative = float(input("Negative: "))
	q = positive/(positive + negative)
	print(q)
	if(q == 1.0 or q == 0.0):
		I = 0
	else:
		I = -1*q*math.log(q,2)-(1-q)*math.log(1-q,2)
	print("q: " + str(q) + "\tI: " + str(I))
