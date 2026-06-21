import {
  Paper,
  Title,
  Container,
  Stack,
  TextInput,
  Select,
  Button,
  Text,
} from "@mantine/core";
import { useForm, schemaResolver } from "@mantine/form";
import { useMutation } from "@tanstack/react-query";
import { Link } from "react-router-dom";

import {
  UserInviteRequest,
  UserInviteRequestBody,
  UserInviteResponse,
  userInviteRequestBodySchema,
} from "./invite.schema";
import { allUserTypesWithLabel } from "../../common/schemas";
import userService from "../userService";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface InviteProps {}

// eslint-disable-next-line no-empty-pattern
function Invite({}: InviteProps) {
  const initialValues: UserInviteRequestBody = {
    email: "",
    userType: "Basic",
  };
  const form = useForm<UserInviteRequestBody>({
    validate: schemaResolver(userInviteRequestBodySchema),
    initialValues,
  });

  function onInviteSuccess() {}

  const mutation = useMutation<UserInviteResponse, unknown, UserInviteRequest>({
    mutationFn: userService.inviteUser,
    onSuccess: onInviteSuccess,
  });

  return (
    <Container p={0} maw={300}>
      <Title>Invite User</Title>
      <Paper shadow="xl" p={24} radius={10}>
        <form
          onSubmit={form.onSubmit((data) =>
            mutation.mutateAsync({ body: data }),
          )}
        >
          <Stack>
            <TextInput
              label="Email"
              style={{ textAlign: "left" }}
              placeholder="Email"
              {...form.getInputProps("email")}
            />
            <Select
              label="User Type"
              {...form.getInputProps("userType")}
              style={{ textAlign: "left" }}
              data={Object.values(allUserTypesWithLabel).filter(
                (x) => x.value !== "Guest",
              )}
              allowDeselect={false}
            />
            <Button type="submit">Submit</Button>
            {mutation.isSuccess && (
              <Text>
                {
                  "Invitation successfully created. To accept the invite, click "
                }
                <Link
                  to={`accept?email=${mutation.data.email}&token=${mutation.data.token}`}
                >
                  here
                </Link>
                .
              </Text>
            )}
          </Stack>
        </form>
      </Paper>
    </Container>
  );
}

export default Invite;
