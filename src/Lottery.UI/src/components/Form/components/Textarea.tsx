import { Textarea as MantineTextarea } from "@mantine/core";
import { useController } from "react-hook-form";
import { TextareaProps, WithName } from "../types";

function Textarea(props: WithName<TextareaProps>) {
  const { label, name, ...rest } = props;
  const {
    field,
    fieldState: { error: fieldError },
  } = useController({ name });

  return (
    <MantineTextarea
      id={name}
      label={label}
      {...rest}
      {...field}
      error={fieldError?.message?.toString()}
    />
  );
}

export default Textarea;
