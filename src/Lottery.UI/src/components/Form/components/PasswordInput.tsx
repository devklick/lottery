import { PasswordInput as _PasswordInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { PasswordInputProps, WithName } from "../types";
import ErrorMessage from "./ErrorMessage";

function PasswordInput(props: WithName<PasswordInputProps>) {
  const { label, name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  const error = fieldError ? (
    <ErrorMessage>{fieldError.message?.toString()}</ErrorMessage>
  ) : undefined;

  return (
    <_PasswordInput
      id={name}
      label={label}
      error={error}
      {...rest}
      {...field}
    />
  );
}

export default PasswordInput;
