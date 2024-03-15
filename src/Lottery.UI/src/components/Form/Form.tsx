import { zodResolver } from "@hookform/resolvers/zod";
import { Grid, GridColProps } from "@mantine/core";
import { FieldValues, FormProvider, useForm } from "react-hook-form";
import { z } from "zod";
import { ControllerProps } from "./types";
import FormInput from "./components/FormInput";

interface FormProps<Fields extends FieldValues> {
  schema: z.Schema;
  fields: Record<keyof Fields, ControllerProps & { col?: GridColProps }>;
  onSubmit: (data: Fields) => void;
}

function Form<Fields extends FieldValues>({
  schema,
  fields,
  onSubmit,
}: FormProps<Fields>) {
  const methods = useForm<Fields>({
    resolver: zodResolver(schema),
  });

  return (
    <FormProvider {...methods}>
      <form onSubmit={methods.handleSubmit(async (data) => onSubmit(data))}>
        <Grid justify="center" gutter={"xl"}>
          {Object.entries(fields).map(([fieldName, field]) => {
            return (
              <Grid.Col {...field.col} key={fieldName}>
                <FormInput {...field} name={fieldName} />
              </Grid.Col>
            );
          })}
        </Grid>
      </form>
    </FormProvider>
  );
}

export default Form;
