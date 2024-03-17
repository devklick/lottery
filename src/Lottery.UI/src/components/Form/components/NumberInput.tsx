import { NumberInput as MantineNumberInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { NumberInputProps, WithName } from "../types";

function NumberInput(props: WithName<NumberInputProps>) {
  const { label, name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <MantineNumberInput
      id={name}
      label={label}
      onChange={(value) => {
        if (value === "") {
          /**
           * @see https://mantine.dev/core/number-input/#input-value-type
           */
          onChange(defaultValues?.[name] || "");
        } else {
          onChange(value);
        }
      }}
      error={fieldError?.message?.toString()}
      {...rest}
      {...restField}
    />
  );
}

export default NumberInput;
