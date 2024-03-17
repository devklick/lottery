import { DateInput as MantineDateInput } from "@mantine/dates";
import { useController } from "react-hook-form";
import { DateInputProps, WithName } from "../types";

function DatePickerInput(props: WithName<DateInputProps>) {
  const { label, name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  return (
    <MantineDateInput
      id={name}
      label={label}
      error={fieldError?.message?.toString()}
      {...rest}
      {...field}
      type={undefined} // disables the native date picker
    />
  );
}

export default DatePickerInput;
