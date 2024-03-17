import { Input, PinInput as _PinInput } from "@mantine/core";
import { useController } from "react-hook-form";
import { PinInputProps, WithName } from "../types";

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

  return (
    <Input.Wrapper
      id={name}
      label={label}
      error={fieldError?.message?.toString()}
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
      <_PinInput id={name} error={!!fieldError?.message} {...rest} {...field} />
    </Input.Wrapper>
  );
}

export default PinInput;
