from argparse import *






buffTypes = ["Buff", "Debuff"]
modules = ["HoT", "DoT", "Invigorate", "Exhaust", "FortifyAttribute", "DamageAttribute"]

parser = ArgumentParser(description='Generates Buffs')
parser.add_argument('cName', action='store', help="Name of the class for the buff (cannot contain spaces)")
parser.add_argument('bName', action='store', help="Name of the buff (can contain spaces)")
parser.add_argument('bType', action='store', choices=buffTypes, help="Specifies Buff or Debuff")
parser.add_argument('mod', action='store', choices=modules, help="Module for the buff to use.")
parser.add_argument('effectThing', action='store',  help="Thing the buff will affect")
parser.add_argument('magnitude', action='store', help="Magnitude of the effect")
newBuff = parser.parse_args()


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
#tempText.replace("BUFF_NAME", newBuff.bName, 5)
#tempText.replace("THE_ACTUAL_BUFF", actualBuff, 5)
#tempText.replace("BUFF_TYPE", buffType, 5)

newBuffFile = open(newBuff.cName+".cs", "w")

newBuffFile.write(modText)

template.close()
newBuffFile.close()