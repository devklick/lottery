import { TextInput as _TextInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { TextInputProps, WithName } from "../types";

function TextInput(props: WithName<TextInputProps>) {
  const { name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  return (
    <_TextInput
      id={name}
      error={fieldError?.message?.toString()}
      {...field}
      {...rest}
      value={undefined}
    />
  );
}

export default TextInput;
