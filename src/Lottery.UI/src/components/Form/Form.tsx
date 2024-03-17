import { zodResolver } from "@hookform/resolvers/zod";
import { Button, Grid, GridColProps } from "@mantine/core";
import {
  DefaultValues,
  FieldValues,
  FormProvider,
  UseFormSetValue,
  useForm,
} from "react-hook-form";
import { z } from "zod";
import { ControllerProps } from "./types";
import FormInput from "./components/FormInput";
import { DateInput, getFormattedDate } from "@mantine/dates";
import { PropsWithChildren, useEffect } from "react";

interface FormProps<Fields extends FieldValues> {
  schema: z.Schema;
  fields: {
    [P in keyof Fields]: ControllerProps & {
      col?: GridColProps;
      defaultValue?: Fields[P];
    };
  };
  onSubmit: (data: Fields) => void;
}

function Form<Fields extends FieldValues>({
  schema,
  fields,
  children,
  onSubmit,
}: PropsWithChildren<FormProps<Fields>>) {
  const methods = useForm<Fields>({
    resolver: zodResolver(schema),
  });

  useEffect(() => {
    console.log(methods.getValues());
  }, [methods]);

  return (
    <FormProvider {...methods}>
      <form onSubmit={methods.handleSubmit(async (data) => onSubmit(data))}>
        <Grid justify="center" gutter={"xl"}>
          {Object.entries(fields).map(([fieldName, field]) => {
            return (
              <Grid.Col
                key={fieldName}
                span={{ xs: 12, sm: 12, md: 6, lg: 6 }}
                {...field.col}
              >
                <FormInput {...field} name={fieldName} />
              </Grid.Col>
            );
          })}
          {children}
          <Grid.Col
            span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
          >
            <Button
              type="submit"
              loading={methods.formState.isSubmitting}
              fullWidth
            >
              {methods.formState.isSubmitting ? "Submitting" : "Submit"}
            </Button>
          </Grid.Col>
          <Grid.Col
            span={{ xs: 3.5, sm: 2.5, md: 2.5, lg: 2.5, xl: 2.5, mt: 10 }}
          >
            <Button
              fullWidth
              onClick={() => {
                console.log(methods.getValues());
              }}
            >
              Log Form Values
            </Button>
          </Grid.Col>
        </Grid>
      </form>
    </FormProvider>
  );
}

export default Form;
