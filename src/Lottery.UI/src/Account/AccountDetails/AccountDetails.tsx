import {
  Button,
  ButtonGroup,
  Grid,
  Group,
  Text,
  TextInput,
  Tooltip,
  useMantineTheme,
} from "@mantine/core";
import { schemaResolver, useForm } from "@mantine/form";
import { useMediaQuery } from "@mantine/hooks";
import { IconCheck, IconX } from "@tabler/icons-react";
import { useState } from "react";

import PageSection from "../../components/PageSection";
import { useUpdateAccount } from "../account.hooks";
import {
  UpdateAccountRequestBody,
  updateAccountRequestBodySchema,
} from "./updateAccountDetails.schema";

interface AccountDetailsProps {
  username: string;
  email: string;
  phoneNumber?: string;
  emailConfirmed: boolean;
  phoneNumberConfirmed: boolean;
}

export default function AccountDetails({
  username,
  email,
  phoneNumber,
  emailConfirmed,
  phoneNumberConfirmed,
}: AccountDetailsProps) {
  const [editing, setEditing] = useState(false);
  const updateAccount = useUpdateAccount();
  const form = useForm<UpdateAccountRequestBody>({
    validate: schemaResolver(updateAccountRequestBodySchema),
    initialValues: {
      email,
      phoneNumber,
      username,
    },
    validateInputOnChange: true,
  });
  const { breakpoints } = useMantineTheme();
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  const xs = useMediaQuery(`(max-width: ${breakpoints.xs})`);
  return (
    <PageSection
      title="Account Details"
      subheader="Here you can find the basic information account your account"
      collapsable
      footer={
        <Group justify="flex-end" w="100%">
          {!editing && <Button onClick={() => setEditing(true)}>Edit</Button>}
          {editing && (
            <ButtonGroup
              orientation={xs ? "vertical" : "horizontal"}
              w={sm ? "100%" : "auto"}
            >
              <Button
                disabled={updateAccount.isPending}
                type="reset"
                onClick={() => {
                  form.reset();
                  setEditing(false);
                }}
                fullWidth={sm}
                variant="outline"
                color="red"
              >
                Cancel
              </Button>
              <Button
                disabled={!form.isTouched()}
                type="submit"
                variant="gradient"
                gradient={{ from: "blue", to: "grape", deg: 20 }}
                fullWidth={sm}
                form={"update-account"}
              >
                Submit
              </Button>
            </ButtonGroup>
          )}
        </Group>
      }
    >
      <form
        id="update-account"
        onSubmit={form.onSubmit(async (data) =>
          updateAccount.mutateAsync(data),
        )}
      >
        <Grid w={"100%"} maw={600}>
          <Grid.Col span={5.5}>
            <Text ta={"right"}>Username</Text>
          </Grid.Col>
          <Grid.Col span={5.5}>
            <TextInput
              {...form.getInputProps("username")}
              disabled={!editing}
              ta={"left"}
            />
          </Grid.Col>
          <Grid.Col span={1} />

          <Grid.Col span={5.5}>
            <Text ta={"right"}>Email</Text>
          </Grid.Col>
          <Grid.Col span={5.5}>
            <TextInput
              {...form.getInputProps("email")}
              disabled={!editing}
              ta={"left"}
            />
          </Grid.Col>
          <Grid.Col span={1}>
            {emailConfirmed ? (
              <Tooltip label="Email confirmed">
                <IconCheck size={16} />
              </Tooltip>
            ) : (
              <IconX size={16} />
            )}
          </Grid.Col>

          <Grid.Col span={5.5}>
            <Text ta={"right"}>Phone Number</Text>
          </Grid.Col>
          <Grid.Col span={5.5}>
            <TextInput
              {...form.getInputProps("phoneNumber")}
              disabled={!editing}
              ta={"left"}
            />
          </Grid.Col>
          <Grid.Col span={1}>
            {phoneNumber &&
              (phoneNumberConfirmed ? (
                <Tooltip label="Phone number confirmed">
                  <IconCheck size={16} />
                </Tooltip>
              ) : (
                <IconX size={16} />
              ))}
          </Grid.Col>
        </Grid>
      </form>
    </PageSection>
  );
}
