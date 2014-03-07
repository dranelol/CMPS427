import sys
import os
import random
import math

#DEFINES

minDamage = 10.0
maxDamage = 40.0
attackPower = 1.0
defense = 1.0
magicNumber = 200.0
startHP = 60.0

#ENDDEF

hitCounter=0
counter=0.0

currentHP = startHP

avg=0.0

while(counter < 1000):

	hitCounter=0
	currentHP = startHP


	while(currentHP >0):

		hitCounter+=1
		# DEFAULT COMPUTATION
		range = maxDamage - minDamage
		bias = attackPower - defense
		squareBias = math.pow(bias,2) + magicNumber
		rootBias = math.sqrt(squareBias)
		endResult = (range * bias) / rootBias
		#ENDCOMP
		# FINAL COMPUTATION
		ceiling = maxDamage + endResult

		if(ceiling > maxDamage):
			ceiling = maxDamage

		floor = minDamage + endResult

		if(floor < minDamage):
			floor = minDamage
		#ENDCOMP
		
		ceiling = math.ceil(ceiling)
		floor = math.floor(floor)
		
		final = random.randint(int(floor), int(ceiling))
		
		currentHP -= final
		

	#print("it took " + str(hitCounter) + " hits to kill!")

	
	avg += hitCounter
	
	counter+=1
	
print(avg / 1000.0)



hitCounter=0
counter=0.0

currentHP = startHP + 30

avg=0.0

while(counter < 1000):

	hitCounter=0
	currentHP = startHP + 30

	while(currentHP >0):
		hitCounter+=1
		# DEFAULT COMPUTATION
		range = maxDamage - minDamage
		bias = attackPower - defense
		squareBias = math.pow(bias,2) + magicNumber
		rootBias = math.sqrt(squareBias)
		endResult = (range * bias) / rootBias
		#ENDCOMP
		# FINAL COMPUTATION
		ceiling = maxDamage + endResult

		if(ceiling > maxDamage):
			ceiling = maxDamage

		floor = minDamage + endResult

		if(floor < minDamage):
			floor = minDamage
		#ENDCOMP
		
		ceiling = math.ceil(ceiling)
		floor = math.floor(floor)
		
		final = random.randint(int(floor), int(ceiling))
		
		currentHP -= final
		
	#print("it took " + str(hitCounter) + " hits to kill!")
	
	avg += hitCounter
	
	counter+=1
	
print(avg / 1000.0)

input("press enter")

	
	


