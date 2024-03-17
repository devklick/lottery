import { PasswordInput as _PasswordInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { PasswordInputProps, WithName } from "../types";

function PasswordInput(props: WithName<PasswordInputProps>) {
  const { label, name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  return (
    <_PasswordInput
      id={name}
      label={label}
      error={fieldError?.message?.toString()}
      {...rest}
      {...field}
    />
  );
}

export default PasswordInput;
