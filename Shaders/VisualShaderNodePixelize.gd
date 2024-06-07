@tool
extends VisualShaderNodeCustom
class_name VisualShaderNodePixelize


func _get_name():
	return "Pixelize"


func _get_category():
	return "MyShaderNodes"


func _get_description():
	return "Quantize coordinates based on parameters"


func _init():
	set_input_port_default_value(1, 0.0)


func _get_return_icon_type():
	return VisualShaderNode.PORT_TYPE_VECTOR_2D


func _get_input_port_count():
	return 2


func _get_input_port_name(port):
	match port:
		0:
			return "uv"
		1:
			return "amount"


func _get_input_port_type(port):
	match port:
		0:
			return VisualShaderNode.PORT_TYPE_VECTOR_2D
		1:
			return VisualShaderNode.PORT_TYPE_SCALAR


func _get_output_port_count():
	return 1


func _get_output_port_name(_port):
	return "result"


func _get_output_port_type(_port):
	return VisualShaderNode.PORT_TYPE_VECTOR_2D


func _get_global_code(_mode):
	return """
		float floatPixelate(float f, float amount) {
			return floor(f * amount) / amount;
		}

		vec2 pixelate(vec2 P, float amount) {
			return vec2(floatPixelate(P.x, amount), floatPixelate(P.y, amount));
		}
	"""


func _get_code(input_vars, output_vars, _mode, _type):
	return output_vars[0] + " = pixelate(%s.xy, %s);" % [input_vars[0], input_vars[1]]
