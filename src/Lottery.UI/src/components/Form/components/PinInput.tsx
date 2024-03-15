import { Input, PinInput as _PinInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { PinInputProps, WithName } from "../types";
import ErrorMessage from "./ErrorMessage";

function PinInput(props: WithName<PinInputProps>) {
  const {
    label,
    name,
    description,
    descriptionProps,
    required,
    withAsterisk,
    labelProps,
    errorProps,
    inputContainer,
    inputWrapperOrder,
    ...rest
  } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  const errorMessage = fieldError ? (
    <ErrorMessage>{fieldError.message?.toString()}</ErrorMessage>
  ) : undefined;

  return (
    <Input.Wrapper
      id={name}
      label={label}
      error={errorMessage}
      {...{
        description,
        descriptionProps,
        required,
        withAsterisk,
        labelProps,
        errorProps,
        inputContainer,
        inputWrapperOrder,
      }}
    >
      <_PinInput id={name} error={!!errorMessage} {...rest} {...field} />
    </Input.Wrapper>
  );
}

export default PinInput;
