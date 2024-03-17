import CheckboxGroup from "./CheckboxGroup";
import { ControllerProps } from "../types";
import DatePickerInput from "./DateInput";
import MultiSelect from "./MultiSelect";
import NumberInput from "./NumberInput";
import PasswordInput from "./PasswordInput";
import PinInput from "./PinInput";
import RadioGroup from "./RadioGroup";
import Select from "./Select";
import SwitchGroup from "./SwitchGroup";
import Textarea from "./Textarea";
import TextInput from "./TextInput";
import Switch from "./Switch";

type FormFieldProps = ControllerProps & {
  name: string;
};

function FormInput(props: FormFieldProps) {
  switch (props.control) {
    case "checkbox-group":
      return <CheckboxGroup {...props} />;
    case "date-input":
      return <DatePickerInput {...props} />;
    case "multi-select":
      return <MultiSelect {...props} />;
    case "number-input":
      return <NumberInput {...props} />;
    case "password-input":
      return <PasswordInput {...props} />;
    case "pin-input":
      return <PinInput {...props} />;
    case "radio-group":
      return <RadioGroup {...props} />;
    case "select":
      return <Select {...props} />;
    case "switch":
      return <Switch {...props} />;
    case "switch-group":
      return <SwitchGroup {...props} />;
    case "text-area":
      return <Textarea {...props} />;
    case "text-input":
      return <TextInput {...props} />;
    case "custom-element":
      return props.element;
    default:
      return null;
  }
}

export default FormInput;
