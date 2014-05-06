from argparse import *



class BuffException(Exception):
	pass

buffTypes = ["Buff", "Debuff"]
modules = ["HoT", "DoT", "Invigorate", "Exhaust", "FortifyAttribute", "DamageAttribute"]
modtypeStats = ["Percentage","Value"]
damageTypeStats = ["AIR", "EARTH", "FIRE", "NONE", "PHYSICAL", "SHADOW", "WATER"]
attStats = ["ATTACK_SPEED", "DEFENSE", "HEALTH", "MAX_DAMAGE", "MIN_DAMAGE", "MOVEMENT_SPEED", "POWER", "RESOURCE"]

parser = ArgumentParser(description="Generates Buffs")
parser.add_argument('inputFile', action="store", help="File containing a | separated list of buffs.")

newBuff = parser.parse_args()

aurasDict = ""

template = open("template_file", "r")
tempText = template.read()
template.close()

with open(newBuff.inputFile) as f:
	for line in f:
		inputList = line.split("|")
		iCount = 0
		for i in inputList:
			inputList[iCount] = i.strip()
			iCount+=1


		if len(inputList) != 12:
			raise BuffException("Not enough fields for "+inputList[0])
		elif inputList[4] not in modules:
			raise BuffException("The module given for " +inputList[0]+ " does not exist!")
		elif " " in inputList[0]:
			raise BuffException("The class name given for " +inputList[0]+ " must not contain spaces.")
		elif inputList[3] not in buffTypes:
			raise BuffException("The buff type given for "+inputList[0]+" is not valid. Must be Buff or Debuff")
		elif inputList[4] == "HoT" and inputList[5] not in modtypeStats:
			raise BuffException("The ModType given for "+inputList[0]+" is not valid!")
		elif inputList[4] == "DoT" and inputList[5] not in damageTypeStats:
			raise BuffException("The DamageType given for "+inputList[0]+" is not valid!")
		elif (inputList[4] == "Exhaust" or inputList[4] == "Invigorate") and inputList[5] != "":
			raise BuffException("An attribute was given in field six for "+inputList[0]+" where it was not needed. Please leave this field blank.")
		elif (inputList[4] == "FortifyAttribute" or inputList[4] == "DamageAttribute") and inputList[5] not in attStats:
			raise BuffException("The attribute given for "+inputList[0]+" is not valid!")
		

		if inputList[4] == "HoT":
			actualBuff = inputList[4]+"(ModType."+inputList[5]+","+inputList[6]+"f)"
		elif inputList[4] == "DoT":
			actualBuff = inputList[4]+"(DamageType."+inputList[5]+","+inputList[6]+"f)"
		elif inputList[4] == "Invigorate":
			actualBuff = inputList[4]+"("+inputList[6]+"f)"
		elif inputList[4] == "Exhaust":
			actualBuff = inputList[4]+"("+inputList[6]+"f)"
		elif inputList[4] == "FortifyAttribute":
			actualBuff = inputList[4]+"(Attributes.Stats."+inputList[5]+","+inputList[6]+"f)"
		elif inputList[4] == "DamageAttribute":
			actualBuff = inputList[4]+"(Attributes.Stats."+inputList[5]+","+inputList[6]+"f)"
		
		
		aurasDict += "Auras[\""+inputList[0]+"\"] = new "+inputList[0]+"(\""+inputList[0]+"\");\n"
		
		
		buffType = "AuraType."+inputList[3]
		
		
		modText = tempText.replace("BUFF_CLASS_NAME", inputList[0], 5).replace("BUFF_NAME", inputList[1], 5).replace("THE_ACTUAL_BUFF", actualBuff, 5).replace("BUFF_TYPE", buffType, 5).replace("BUFF_FLAVOR_TEXT", inputList[2], 5).replace("ICON_TEXTURE_FILE_NAME", inputList[7], 5).replace("PARTICLE_EFFECT_NAME", inputList[8], 5).replace("BUFF_MAX_STACKS", inputList[9], 5).replace("BUFF_MIN_STACKS", inputList[10], 5).replace("BUFF_DURATION", inputList[11], 5)
		
		newBuffFile = open(inputList[0]+".cs", "w")
		
		newBuffFile.write(modText)
		
		
		newBuffFile.close()
		

auraDictFile = open("newAuraDictionaryEntries", "w");

auraDictFile.write(aurasDict)

auraDictFile.close()

'''
Old Single Buff Creation Code

parser.add_argument('cName', action='store', help="Name of the class for the buff (cannot contain spaces)", required=False)
parser.add_argument('bName', action='store', help="Name of the buff (can contain spaces)", required=False)
parser.add_argument('bType', action='store', choices=buffTypes, help="Specifies Buff or Debuff", required=False)
parser.add_argument('mod', action='store', choices=modules, help="Module for the buff to use.", required=False)
parser.add_argument('effectThing', action='store',  help="Thing the buff will affect", required=False)
parser.add_argument('magnitude', action='store', help="Magnitude of the effect", required=False)
template = open("template_file", "r")

tempText = template.read()

if newBuff.mod == "HoT":
	actualBuff = newBuff.mod+"(newBuff.modType."+newBuff.effectThing+","+newBuff.magnitude+"f)"
elif newBuff.mod == "DoT":
	actualBuff = newBuff.mod+"(DamageType."+newBuff.effectThing+","+newBuff.magnitude+"f)"
elif newBuff.mod == "Invigorate":
	actualBuff = newBuff.mod+"("+newBuff.magnitude+"f)"
elif newBuff.mod == "Exhaust":
	actualBuff = newBuff.mod+"("+newBuff.magnitude+"f)"
elif newBuff.mod == "FortifyAttribute":
	actualBuff = newBuff.mod+"(Attributes.Stats."+newBuff.effectThing+","+newBuff.magnitude+"f)"
elif newBuff.mod == "DamageAttribute":
	actualBuff = newBuff.mod+"(Attributes.Stats."+newBuff.effectThing+","+newBuff.magnitude+"f)"




buffType = "AuraType."+newBuff.bType


modText = tempText.replace("BUFF_CLASS_NAME", newBuff.cName, 5).replace("BUFF_NAME", newBuff.bName, 5).replace("THE_ACTUAL_BUFF", actualBuff, 5).replace("BUFF_TYPE", buffType, 5)

newBuffFile = open(newBuff.cName+".cs", "w")

newBuffFile.write(modText)

template.close()
newBuffFile.close()
'''
